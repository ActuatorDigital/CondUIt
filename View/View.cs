using System;
using System.Linq;
using UnityEngine;

namespace Conduit {

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public abstract class ViewBase<M> :
        MonoBehaviour, IView {

        [SerializeField]
        private TransitionHandler[] _transitionHandlers = new TransitionHandler[0];

        protected virtual void OnScreenOrientationChanged(ScreenAspect aspect) { }
        public virtual ScreenAspect CurrentScreenAspect {
            get { return ScreenAspectNotifier.CurrentScreenAspect; }
        }

        public abstract bool IsPartial { get; }

        public object Model { get; set; }
        protected M ViewModel {
            get { return (M)Model; }
            set { Model = value; }
        }

        public virtual void Initialise(ConduitUIFramework framework) {
            ScreenAspectNotifier.OnScreenOrientationChanged += OnScreenOrientationChanged;

            if (_transitionHandlers == null || !_transitionHandlers.Any())
                _transitionHandlers = new TransitionHandler[] {
                    gameObject.AddComponent<OnOffTransition>()
                };
        }

        public void Render() {
            foreach (var handler in _transitionHandlers)
                handler.OnShow();
            LoadElements();
        }

        public void Hide() {
            if (!gameObject.activeInHierarchy)
                return;

            foreach (var handler in _transitionHandlers)
                handler.OnHide();

            if (Model != null)
                ClearElements();
        }

        protected abstract void LoadElements();
        protected abstract void ClearElements();

        public virtual Type GetControllerType() {
            return null;
        }
    }

    public abstract class View<M> : ViewBase<M> {

        public override bool IsPartial => true;

        public override void Initialise(ConduitUIFramework framework) {
            LoadControllers(framework._controllers);
            base.Initialise(framework);
        }

        protected abstract void LoadControllers(IControllerLoader controllers);

    }

    public abstract class View<M, C> :
        ViewBase<M>
        where C : IController {

        public override void Initialise(ConduitUIFramework framework) {
            _controller = framework.GetController<C>();
            base.Initialise(framework);
        }

        IController _controller;
        public C Controller {
            get { return (C)_controller; }
            set { _controller = value; }
        }

        public override Type GetControllerType() {
            return typeof(C);
        }
    }
}