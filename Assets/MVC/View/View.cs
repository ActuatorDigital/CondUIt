using MVC.Controllers;
using UnityEngine;
using MVC.Routing;

namespace MVC.Views
{
    public abstract class View<M, C> : MonoBehaviour, IView where C : IController
    {
        [SerializeField]
        private TransitionHandler[] _transitionHandlers = new TransitionHandler[0];

        private IMvcRouting _routing;
        private object _model;

        public M Model { get { return (M)_model; } }

        public void Init(IMvcRouting routing)
        {
            _routing = routing;
        }

        public void Render(object model)
        {
            _model = model;
            LoadElements();
            foreach (var handler in _transitionHandlers)
                handler.OnShow();
        }

        public void Hide()
        {
            foreach (var handler in _transitionHandlers)
                handler.OnHide();
            ClearElements();
        }

        protected abstract void LoadElements();
        protected abstract void ClearElements();

        protected void Action (string action, params object[] args)
        {
            _routing.Action<C>(action, args);
        }

        protected void Action<T> (string action, params object[] args) where T : IController
        {
            _routing.Action<T>(action, args);
        }
    }
}

