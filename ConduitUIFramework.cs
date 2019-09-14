using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Conduit {

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(ScreenAspectNotifier))]
    [RequireComponent(typeof(StandaloneInputModule))]
    public class ConduitUIFramework : MonoBehaviour {

        private List<IView> _views = new List<IView>();
        private List<IController> _controllers = new List<IController>();

        private void Start() {

            if (_controllers.Any())
                _controllers.Clear();
            if (_views.Any())
                _views.Clear();

            ConnectUIFamework();
            DeliverServices<IFirstController>();
        }

        void ConnectUIFamework() {
            _controllers.AddRange(GetComponentsInChildren<IController>(true));
            _views.AddRange(GetComponentsInChildren<IView>(true));

            foreach (var v in _views) {
                try {
                    v.Initialise(this);
                } catch (MissingComponentException ex) {
                    throw new MissingComponentException(
                        v.GetType().Name + " could not find it's controller", ex);
                }
            }

            HideViews<IFirstController>();
        }

        void DeliverServices<C>() where C : IController {

            var services = FindObjectOfType<ConduitServices>();
            if (services == null)
                services = gameObject.AddComponent<ConduitServices>();

            IController firstController = null;
            foreach (IController c in _controllers) {
                c.LoadServices(services._services);
                c.LoadFramework(this);
                if (c is C)
                    firstController = c as IController;
            }

            firstController.Display();
            services._services.ClearServices();
        }

        internal IController GetController<C>() where C : IController {
            var type = typeof(C);
            var controller = _controllers
                .FirstOrDefault(c => c.GetType().IsAssignableFrom(type));

            if (controller == null)
                throw new MissingComponentException(
                    "Controller " + type.FullName +
                    " not found.");

            return controller;
        }

        internal IView GetView<V>() where V : IView {
            var type = typeof(V);
            var view = _views.FirstOrDefault(v =>
                v.GetType().IsAssignableFrom(type));

            if (view == null)
                throw new MissingComponentException(
                    "View " + type.FullName +
                    " not found.");

            return view;
        }

        internal void HideActiveViews() {
            foreach (var v in _views)
                if (!v.IsPartial)
                    v.Hide();
        }

        internal void HideViews<C>() {
            var controller = _controllers
                .First(c => c is C);
            var exclusiveController = controller.Exclusive;
            foreach (var v in _views) {
                var controllerType = v.GetControllerType();
                var parentController = _controllers
                    .First(c => c.GetType() == controllerType);
                var targetIsParent = parentController.Equals(controller);
                if (controller.Exclusive && !targetIsParent)
                    v.Hide();
            }
        }

    }

}