using System.Linq;
using UnityEngine;

namespace MVC {
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

        public object ViewModel { get; set; }
        protected M Model {
            get { return (M)ViewModel; }
            set { ViewModel = value; }
        }

        public void Initialise (MVCFramework mvc)
        {
            _controller = mvc.GetController<C>();

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
            foreach (var handler in _transitionHandlers)
                handler.OnHide();

            if(ViewModel != null)
                ClearElements();
        }

        protected abstract void LoadElements();
        protected abstract void ClearElements();
    }
}