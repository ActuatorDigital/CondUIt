using System;
using System.Linq;
using UnityEngine;

namespace MVC {

    public abstract class Controller :
        MonoBehaviour, IController{

        internal MVCFramework _framework;
        
        public abstract bool Exclusive { get; }

        public abstract void LoadServices(IServicesLoader services);

        public void LoadFramework(MVCFramework framework) {
            _framework = framework;
        }

        public C Action<C>() where C : Controller {

            CheckFrameworkInitialized();

            Type controllerType = typeof(C);
            return ActivateController(controllerType) as C;

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
                var view = (mbView as IView);
                if (mbView.GetType().IsAssignableFrom(viewType)) {
                    view.ViewModel = viewModel;
                    view.Render();
                    found = true;
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
            if (type.IsAssignableFrom(typeof(T))) {
                var errorMsg = type.FullName +
                    " is not a " + typeof(T).FullName;
                throw new ApplicationException(errorMsg);
            }
        }
    }
    
}