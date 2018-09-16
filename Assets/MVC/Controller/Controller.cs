using System;
using System.Linq;
using UnityEngine;

namespace MVC {

    public abstract class Controller<M> :
            MonoBehaviour, IController {

        internal MVCFramework _framework;

        public abstract bool Exclusive { get; }

        public abstract void LoadServices(IServicesLoader services);

        // public abstract void Display();

        public void LoadFramework(MVCFramework framework) {
            _framework = framework;
        }

        public C Action<C>(
                // string action,
                // params object[] args
                ) where C : class, IController {

            CheckFrameworkInitialized();

            Type controllerType = typeof(C);
            return ActivateController(controllerType) as C;

            // if (controller == null)
            //     throw new MissingComponentException(
            //         "Controller " + controllerType.GetType().FullName +
            //         " not found. Failed to Call action " + action + ".");

            // var method = controllerType.GetMethod(action);
            // if (method == null)
            //     throw new MissingMethodException(
            //         "Action " + action + " not found on " +
            //         controllerType.GetType().FullName + "." +
            //         " Failed to call action " + action + ".");
            
            // method.Invoke(controller, args);

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
            CheckFrameworkInitialized();

            Type viewType = typeof(V);

            bool found = false;
            foreach (MonoBehaviour mbView in _framework.Views) {
                if (mbView.GetType().IsAssignableFrom(viewType)) {
                    var view = (mbView as IView);
                    view.ViewModel = viewModel;
                    mbView.gameObject.SetActive(true);
                    view.Render();
                    found = true;
                } else {
                    // TODO: Handling of partial views.
                    mbView.gameObject.SetActive(false);
                }
            }

            if(!found)
                throw new MissingComponentException(
                    "Failed to Display " + viewType.FullName +
                    ", View not found.");

        }

        void CheckFrameworkInitialized() {
            if (_framework == null)
                throw new TypeInitializationException(
                    typeof(MVCFramework).FullName,
                    new Exception("Calling action before framework has been Initialized, run " +
                    "MVCFramweork.Initialize with the default controller before calling actions.")
                );
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