using System.Linq;
using UnityEngine;

namespace MVC {
    public abstract class View<M, C> :
        MonoBehaviour, IView
        where C : IController {

        [SerializeField]
        private TransitionHandler[] _transitionHandlers = new TransitionHandler[0];

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

        private void Awake() {
            ConnectMVC();

            if (_transitionHandlers == null || !_transitionHandlers.Any())
                _transitionHandlers = new TransitionHandler[] {
                    gameObject.AddComponent<OnOffTransition>()
                };
        }

        private IController GetController(GameObject go) {
            return go.GetComponent<C>() as IController;
        }

        public void ConnectMVC() {
            _controller = FindParentController();
            if (Controller == null)
                throw new MissingComponentException(
                    GetType().FullName + " could not find" +
                    " it's controller " + typeof(C).FullName +
                    " on GameObject " + gameObject.name +
                    " or on " + gameObject.transform.parent.name + "."
                );
        }

        public IController FindParentController() {
            IController controller;
            var searchTarget = gameObject;
            do {
                controller = GetController(searchTarget);
                if (controller == null)
                    searchTarget = searchTarget.transform.parent.gameObject;
            } while (controller == null && searchTarget.transform.parent != null);

            return controller;
        }

        public void Render() {
            foreach (var handler in _transitionHandlers)
                handler.OnShow();
            LoadElements();
        }

        public void Hide() {
            foreach (var handler in _transitionHandlers)
                handler.OnHide();
            ClearElements();
        }

        protected abstract void LoadElements();
        protected abstract void ClearElements();
    }
}