using System;
using System.Linq;
using UnityEngine;

namespace Conduit {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public abstract class View<M, C> :
        MonoBehaviour, IView
        where C : IController {

        [SerializeField]
        private TransitionHandler[] _transitionHandlers = new TransitionHandler[0];

        public abstract bool IsPartial { get; }

        IController _controller;
        public C Controller {
            get { return (C)_controller; }
            set { _controller = value; }
        }

        public object Model { get; set; }
        protected M ViewModel {
            get { return (M)Model; }
            set { Model = value; }
        }

        public void Initialise (ConduitUIFramework framework)
        {
            _controller = framework.GetController<C>();

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
            if (!gameObject.activeInHierarchy) return;
            
            foreach (var handler in _transitionHandlers)
                handler.OnHide();

            if(Model != null)
                ClearElements();
        }

        protected abstract void LoadElements();
        protected abstract void ClearElements();

        public Type GetControllerType()
        {
            return typeof(C);
        }
    }
}