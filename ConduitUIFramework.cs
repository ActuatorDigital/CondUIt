using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        internal IControllerLoader _controllers = new ControllerLoader();
        //private List<IController> _controllers = new List<IController>();

        private void Start() {

            //if (_controllers.Any())
            //    _controllers.Clear();
            //if (_views.Any())
            //    _views.Clear();

            ConnectUIFamework();
            DeliverServices();
            //DeliverControllers();
        }

        void ConnectUIFamework() {
            _controllers.Register(GetComponentsInChildren<IController>(true));
            _views.AddRange(GetComponentsInChildren<IView>(true));

            foreach (var v in _views) {
                try {
                    v.Initialise(this);
                } catch (MissingComponentException ex) {
                    throw new MissingComponentException(
                        v.GetType().Name + " could not find it's controller", ex);
                }
            }

            var initialController = _controllers.GetInitialController();
            HideViews(initialController.GetType());
        }

        void DeliverServices() {

            var services = FindObjectOfType<ConduitServices>();
            if (services == null)
                services = gameObject.AddComponent<ConduitServices>();

            IController initialController = null;

            //var sb = new StringBuilder();
            //foreach (var controller in _controllers) {
            //    sb.Append(controller.GetType() + " " + (controller is IController) + " ");
            //}

            //var results = sb.ToString();
            //bool anyNotControllers = _controllers.All(c => c is IController);

            foreach (IController controller in _controllers) {
                controller.LoadServices(services._services);
                controller.LoadFramework(this);
                if (controller is IInitialController)
                    initialController = controller;
            }

            initialController.Display();
            services._services.ClearServices();
        }

        internal IController GetController<C>() where C : IController {
            var type = typeof(C);
            var controller = _controllers.LoadController<C>();

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

        internal void HideViews<C>() where C : IController {
            var controllerType = typeof(C);
            HideViews(controllerType);
        }

        internal void HideViews(Type controllerType) {
            var targetController = _controllers.LoadController(controllerType);
            foreach (var view in _views) {

                var viewController = view.GetControllerType();
                var isControllerlessView = viewController == null;
                if (isControllerlessView) {
                    view.Hide();
                    continue;
                } else {
                    //var parentController = _controllers
                    //    .LoadController(controllerType);
                    //if (targetController == null) {
                    //    view.Hide();
                    //} else {
                        var targetIsParent = viewController.Equals(targetController);
                        if (targetController.Exclusive && !targetIsParent)
                            view.Hide();
                    //}
                }

            }
        }

    }

}