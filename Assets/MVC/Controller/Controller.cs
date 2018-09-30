using System;
using System.Linq;
using UnityEngine;

namespace MVC {

    public abstract class Controller<M> : 
        Controller where M : IModel {

        IModel _context;
        public M Context {
            get {
                try {
                    return (M)_context;
                } catch (InvalidCastException) {
                    var message = GetType().FullName + " given context " +
                        "of type " + _context.GetType().FullName + " but " +
                        "requires a object of type " + typeof(M).FullName + ".";
                    throw new InvalidCastException(message);
                }
            }
            set { _context = value as IModel; }
        }

        public C Redirect<C>(
            IModel context
            // params object[] args
        ) where C : class, IController {

            var targetController = _framework.GetController<C>();
            if(!targetController.GetType().IsSubclassOf(GetType()))
                targetController.GetType().GetMethod("Init").Invoke(context, null);

            base.Redirect<C>();
            return targetController as C;
        }

        public void Init(IModel context) {
            _context = context;
        }

    }

    [RequireComponent(typeof(RectTransform))]
    public abstract class Controller :
            MonoBehaviour, IController {

        internal MVCFramework _framework;

        public abstract bool Exclusive { get; }

        public abstract void LoadServices(IServiceLoader services);

        public abstract void Display();

        public void LoadFramework(MVCFramework framework) {
            _framework = framework;
        }

        public C Redirect<C>(/*IModel context*/) where C : class, IController {
            CheckFrameworkInitialized(); 
            var targetController = _framework.GetController<C>();

            if(targetController.Exclusive)
                _framework.HideViews(); 

            // foreach(var view in _framework.GetViewsForController<C>())
            //     view.Render();

            // targetController.Init(context);
            targetController.Display();

            return targetController as C;
        }

        public void View<V>(object viewModel = null) where V : IView {
            CheckFrameworkInitialized();
            var view = _framework.GetView<V>();

            if(!view.IsPartial)
                _framework.HideActiveViews();

            view.ViewModel = viewModel;
            view.Render();
        }

        protected void CheckFrameworkInitialized() {
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