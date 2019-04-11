using UnityEngine;
using System;

namespace Conduit {

    public abstract class FirstController : 
        Controller, IFirstController {}

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
        ) where C : class, IController {

            var targetController = _framework.GetController<C>();
            if (!targetController.GetType().IsSubclassOf(GetType()))
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

        internal ConduitUIFramework _framework;

        public abstract bool Exclusive { get; }

        public abstract void LoadServices(IServiceLoader services);

        public abstract void Display();

        public void LoadFramework(ConduitUIFramework framework) {
            _framework = framework;
        }

        public C Redirect<C>() where C : class, IController {
            CheckFrameworkInitialized();
            var targetController = _framework.GetController<C>();

            _framework.HideViews<C>();
            targetController.Display();

            return targetController as C;
        }

        public void View<V>(object viewModel = null) where V : IView {
            CheckFrameworkInitialized();
            var view = _framework.GetView<V>();

            if (!view.IsPartial)
                _framework.HideActiveViews();

            view.Model = viewModel;
            view.Render();
        }

        protected void CheckFrameworkInitialized() {
            if (_framework == null)
                throw new TypeInitializationException(
                    typeof(ConduitUIFramework).FullName,
                    new Exception("Calling action before framework has been Initialized, run " +
                    "ConduitFramework.Initialize with the default controller before calling actions.")
                );
        }

    }

}