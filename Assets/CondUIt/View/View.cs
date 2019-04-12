using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Conduit {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public abstract class View<C> :
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

        public dynamic ViewModel { get; set; }
        public ViewBinding ViewBinding { get; set; }

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

            if(ViewModel != null)
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