using System;
using UnityEngine;

namespace Conduit {

    public abstract class InitialController<M> :
        Controller<M>, IInitialController where M : IContext { }

    public abstract class InitialController :
        Controller, IInitialController { }

    /// <summary>
    /// Controllers handle UI routing and connect the injected service layer to the view layer.
    /// They effect UI flow with routes, and convert Data Transfer Objects (DTO)
    /// into View Model (VM) objects for display in views, and handle final view outputs.
    /// </summary>
    /// <typeparam name="M">The controller's context model type.</typeparam>
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

        /// <summary>
        /// Redirect UI flow to another controller.
        /// </summary>
        /// <typeparam name="C">Type of the controller to redirect to.</typeparam>
        /// <param name="context">The context for that controller.</param>
        /// <returns></returns>
        public C Route<C>(
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

            base.Route<C>();
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

        public abstract void LoadServices(IServiceLoader services);

        public abstract void Routed();

        public void LoadFramework(ConduitUIFramework framework) {
            _framework = framework;
        }

        public C Route<C>() where C : class, IController {
            CheckFrameworkInitialized();
            var targetController = _framework.GetController<C>();

            _framework.HideViews<C>();
            targetController.Routed();

            return targetController as C;
        }

        public V View<V>(object viewModel = null) where V : IView {
            CheckFrameworkInitialized();
            var view = _framework.GetView<V>();

            if (view.HideNeighbours)
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