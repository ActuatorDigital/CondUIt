using MVC.Controllers;
using System.Linq;
using UnityEngine;

namespace MVC.Views
{
    public abstract class View<M, C> : MonoBehaviour, IView where C : IController
    {
        [SerializeField]
        private TransitionHandler[] _transitionHandlers = new TransitionHandler[0];

        IController _controller;
        public C Controller
        {
            get { return (C)_controller; }
            set { _controller = value; }
        }

        public object ViewModel { get; set; }
        internal M Model
        {
            get { return (M)ViewModel; }
            set { ViewModel = value; }
        }

        private void Awake()
        {
            ConnectMVC();
        }

        private IController GetController(GameObject go)
        {
            return go.GetComponents(typeof(IController))
                .FirstOrDefault() as IController;
        }

        private void BringToFront()
        {
            var controller = transform.parent.GetComponent(typeof(IController));
            if (controller != null) transform.parent.SetAsLastSibling();

            transform.SetAsLastSibling();
        }

        public void ConnectMVC()
        {
            _controller = GetController(gameObject);
            if (_controller == null)
                _controller = GetController(transform.parent.gameObject);
            if (Controller == null)
                throw new MissingComponentException(
                    GetType().FullName + " could not find" +
                    " it's controller " + typeof(C).FullName +
                    " on GameObject " + gameObject.name +
                    " or on " + gameObject.transform.parent.name + "."
                );
        }

        public void Render()
        {
            LoadElements();
            BringToFront();
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
    }
}

