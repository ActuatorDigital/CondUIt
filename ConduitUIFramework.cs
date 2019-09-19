﻿using System;
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
            GetComponent<ScreenAspectNotifier>().Initialize();
        }

        void DeliverServices() {

            var services = FindObjectOfType<ConduitServices>();
            if (services == null)
                services = gameObject.AddComponent<ConduitServices>();

            IController initialController = null;
            foreach (IController controller in _controllers) {
                controller.LoadServices(services._services);
                controller.LoadFramework(this);
                if (controller is IInitialController)
                    initialController = controller;
            }

            initialController.Routed();
            services._services.ClearServices();
        }

        internal IController GetController<C>() where C : IController {
            var type = typeof(C);
            var controller = _controllers.UseController<C>();

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
            foreach (IView view in _views) {
                (bool isUncontrolled, bool hideOnRoute) = IsUncontrolledView(view);
                if (hideOnRoute) 
                    view.Hide();
            }
        }

        internal void HideViews<C>() where C : IController {
            var controllerType = typeof(C);
            HideViews(controllerType);
        }

        internal void HideViews(Type controllerType) {
            var targetController = _controllers.UseController(controllerType);
            foreach (var view in _views) {
                (bool isUncontrolled, bool hideOnRoute) = IsUncontrolledView(view);
                if (isUncontrolled) {
                    if(hideOnRoute) view.Hide();
                } else {
                    var viewController = view.GetControllerType();
                    var targetIsParent = viewController.Equals(targetController);
                    if (!targetIsParent) view.Hide();
                }

            }
        }

        (bool, bool) IsUncontrolledView(IView view) {
            var viewType = view.GetType();
            var isUncontrolled = viewType.BaseType.IsGenericType &&
                viewType.BaseType.GetGenericTypeDefinition() == typeof(View<>);
            if (!isUncontrolled) return (isUncontrolled, true);

            bool hideOnRoute = (bool)viewType
                .GetProperty("HideMeOnRoute")
                .GetValue(view);
            return (isUncontrolled, hideOnRoute);
        }

    }

}