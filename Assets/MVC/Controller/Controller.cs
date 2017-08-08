using System;
using System.Linq;
using UnityEngine;

namespace MVC {

    public abstract class Controller<M> :
            MonoBehaviour, IController
            where M : IModel {

        internal MVCFramework _framework;
        
        IModel _context;
        public M Context {
            get { return (M)_context; }
            set { _context = value as IModel; }
        }

        public abstract bool Exclusive { get; }

        public abstract void LoadServices(IServicesLoader services);

        public abstract void Display();

        public void LoadFramework(MVCFramework framework) {
            _framework = framework;
        }

        public void Init(IModel context) {
            _context = context;
        }

        public void SaveContext() {
            _framework.OnSaveChanges.Invoke(_context);
        }

        public void Action<C>(
                string action,
                IModel context,
                params object[] args) where C : IController {

            Type controllerType = typeof(C);
            MonoBehaviour controller = ActivateController(controllerType);

            if (_framework.OnSaveChanges == null)
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

            (controller as IController).Init(context);
            method.Invoke(controller, args);

        }

        private MonoBehaviour ActivateController(Type controllerType) {
            var controller = _framework.Controllers.FirstOrDefault(
                c => c.GetType().IsAssignableFrom(controllerType));
            if ((controller as IController).Exclusive)
                _framework.HideViews();
            var controllerViews = controller.gameObject
                .GetComponentsInChildren<MonoBehaviour>()
                .Where(v => v is IView);
            foreach (var view in controllerViews)
                view.gameObject.SetActive(true);
            return controller;
        }

        public void View<V>(object viewModel = null) where V : IView {
            Type viewType = typeof(V);

            foreach (MonoBehaviour mbView in _framework.Views) {
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
    
}