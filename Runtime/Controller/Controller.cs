using System;
using UnityEngine;

namespace AIR.Conduit {

    public abstract class FirstController<M> :
        Controller<M>, IFirstController where M : IContext { }

    public abstract class FirstController :
        Controller, IFirstController { }

    public abstract class Controller<M> :
        Controller where M : IContext {

        IContext _context;
        public M Context {
            get {
                try {
                    return (M)_context;
                } catch (InvalidCastException) {
                    throw ThrowTypeError(GetType(), _context.GetType(), typeof(M));
                }
            }
            set { _context = value as IContext; }
        }

        private InvalidCastException ThrowTypeError(Type controller, Type expected, Type actual) {
            var message = controller.FullName + " given context " +
                "of type " + actual.FullName + " but " +
                "requires a object of type " + expected.FullName + ".";
            return new InvalidCastException(message);
        }

        public C Redirect<C>(
            IContext context
        ) where C : class, IController {

            var targetController = _framework.GetController<C>();
            if (!targetController.GetType().IsSubclassOf(GetType())) {
                var method = targetController.GetType()
                    .GetMethod("Init");
                var paramType = method.GetParameters()[0].ParameterType;
                if (context.GetType() != paramType)
                    throw ThrowTypeError(typeof(C), paramType, context.GetType());
                method.Invoke(targetController, new[] { context });
            }

            base.Redirect<C>();
            return targetController as C;
        }

        public void Init(M context) {
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

        public V View<V>(object viewModel = null) where V : IView {
            CheckFrameworkInitialized();
            var view = _framework.GetView<V>();

            if (!view.IsPartial)
                _framework.HideActiveViews();

            view.Model = viewModel;
            view.Render();

            return (V)view;
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