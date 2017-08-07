using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC {
    class ControllerReferences {
        public static List<MonoBehaviour> Views = new List<MonoBehaviour>();
        public static List<MonoBehaviour> Controllers = new List<MonoBehaviour>();

        public static void HideViews() {
            foreach (MonoBehaviour v in Views)
                v.gameObject.SetActive(false);
        }
    }

    public abstract class Controller<M> :
            MonoBehaviour, IController
            where M : IModel {

        public Action<IModel> OnSaveChanges;

        IModel _context;
        public M Context {
            get { return (M)_context; }
            set { _context = value as IModel; }
        }

        public abstract bool Exclusive { get; }

        private void Start() {
            ConnectMVC();
            ControllerReferences.HideViews();
        }

        public void ConnectMVC() {

            if (ControllerReferences.Controllers.Any())
                ControllerReferences.Controllers.Clear();
            if (ControllerReferences.Views.Any())
                ControllerReferences.Views.Clear();

            var behaviours = gameObject.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (var b in behaviours) {
                if (b.GetType().GetInterfaces().Contains(typeof(IController)))
                    ControllerReferences.Controllers.Add(b);
                if (b.GetType().GetInterfaces().Contains(typeof(IView)))
                    ControllerReferences.Views.Add(b);
            }
        }

        public abstract void Display();
        public virtual void LoadServices(IServicesLoader services) { }

        public void Init(Action<IModel> onSave, IModel context) {
            OnSaveChanges = onSave;
            _context = context;
        }

        public void SaveContext() {
            OnSaveChanges.Invoke(_context);
        }

        public void Action<C>(
                string action,
                IModel context,
                params object[] args) where C : IController {

            Type controllerType = typeof(C);
            MonoBehaviour controller = ActivateController(controllerType);

            if (OnSaveChanges == null)
                throw new MissingComponentException(
                    "OnSaveChanges not set for " + controllerType.GetType().FullName + "." +
                    " Failed to Call action " + action + ".");

            if (controller == null)
                throw new MissingComponentException(
                    "Controller " + controllerType.GetType().FullName +
                    " not found. Failed to Call action " + action + ".");

            var method = controllerType.GetMethod(action);
            if (method == null)
                throw new MissingMethodException(
                    "Action " + action + " not found on " +
                    controllerType.GetType().FullName + "." +
                    " Failed to call action " + action + ".");

            (controller as IController).Init(OnSaveChanges, context);
            method.Invoke(controller, args);

        }

        private MonoBehaviour ActivateController(Type controllerType) {
            var controller = ControllerReferences.Controllers.FirstOrDefault(
                c => c.GetType().IsAssignableFrom(controllerType));
            if ((controller as IController).Exclusive)
                ControllerReferences.HideViews();
            var controllerViews = controller.gameObject
                .GetComponentsInChildren<MonoBehaviour>()
                .Where(v => v is IView);
            foreach (var view in controllerViews)
                view.gameObject.SetActive(true);
            return controller;
        }

        public void View<V>(object viewModel = null) where V : IView {
            Type viewType = typeof(V);

            foreach (MonoBehaviour mbView in ControllerReferences.Views) {
                if (mbView.GetType().IsAssignableFrom(viewType)) {
                    var view = (mbView as IView);
                    view.ViewModel = viewModel;
                    mbView.gameObject.SetActive(true);
                    view.Render();
                    return;
                }
            }

            throw new MissingComponentException(
                "Failed to Display " + viewType.FullName +
                ", View not found.");

        }

        void TypeCheck<T>(Type type) {
            // Check if we've been given a controller.
            if (type.IsAssignableFrom(typeof(T))) {
                var errorMsg = type.FullName +
                    " is not a " + typeof(T).FullName;
                throw new ApplicationException(errorMsg);
            }
        }
        
    }

    public static class GameObjectExtensions {

        public static List<T> FindObjectsOfTypeAll<T>() {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded) {
                    var allGameObjects = s.GetRootGameObjects();
                    for (int j = 0; j < allGameObjects.Length; j++) {
                        var go = allGameObjects[j];
                        results.AddRange(go.GetComponentsInChildren<T>(true));
                    }
                }
            }
            return results;
        }

        public static List<T> FindObjectsOfTypeAll<T>(this GameObject mb) {
            return FindObjectsOfTypeAll<T>();
        }

        public static IController FindControllerOfType(this GameObject mb, Type contextType) {
            var controllerType = typeof(Controller<>).MakeGenericType(contextType);
            var controller = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour))
                .FirstOrDefault(c => c.GetType().IsSubclassOf(controllerType))
                    as IController;
            return controller;
        }

    }
}