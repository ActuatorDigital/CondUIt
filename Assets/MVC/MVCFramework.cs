using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVC {

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(ServiceInitializer))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]
    [RequireComponent(typeof(RectTransform))]
    public class MVCFramework : MonoBehaviour{

        private List<IView> _views = new List<IView>();
        private List<IController> _controllers = new List<IController>();

        ServiceLoader _services = new ServiceLoader();

        public void Initialize<FirstController>() where FirstController : IController 
        {

            if (_controllers.Any())
                _controllers.Clear();
            if (_views.Any())
                _views.Clear();

            ConnectMVC();
            DeliverServices<FirstController>();
        }

        public MVCFramework RegisterService<T>(object service) {
            if (_services.CheckServiceRegistered<T>())
                throw new Exception("Services should be Singletons."+
                    " A service for " + typeof(T).FullName + " is already registered");
            _services.RegisterService(typeof(T), service);
            return this;
        }

        public MVCFramework RegisterService<T>() where T: new() {
            var service = new T();
            _services.RegisterService(service);
            return this;
        }

        void ConnectMVC()  {
            _controllers.AddRange(GetComponentsInChildren<IController>(true));
            _views.AddRange(GetComponentsInChildren<IView>(true));

            foreach (var v in _views){
                try{
                    v.Initialise(this);
                }catch(MissingComponentException ex){
                    throw new MissingComponentException(
                        v.GetType().Name + " could not find it's controller", ex);   
                }
            }

            HideViews();
        }

        void DeliverServices<C>() where C : IController {
            foreach(IController c in _controllers){
                c.LoadServices(_services);
                c.LoadFramework(this);
            }

            foreach (IController c in _controllers) {
                if (c is C) {
                    var firstController = (c as IController);
                    firstController.Display();
                }
            }
            _services.ClearServices();
        }

        internal IController GetController<C>() where C : IController
        {
            var type = typeof(C);
            var controller = _controllers.FirstOrDefault(c => c.GetType().IsAssignableFrom(type));

            if (controller == null)
                throw new MissingComponentException(
                    "Controller " + type.FullName +
                    " not found.");

            return controller;
        }

        internal IView GetView<V>() where V : IView
        {
            var type = typeof(V);
            var view = _views.FirstOrDefault(v => v.GetType().IsAssignableFrom(type));

            if (view == null)
                throw new MissingComponentException(
                    "View " + type.FullName +
                    " not found.");

            return view;
        }

        internal void HideActiveViews ()
        {
            foreach (var v in _views)
                if (!v.IsPartial)
                    v.Hide();
        }

        internal void HideViews()
        {
            foreach (var v in _views){
                var controllerType = v.GetControllerType();
                var controller = _controllers
                    .First( c => c.GetType() == controllerType );
                if(controller.Exclusive)
                    v.Hide();
            }
        }
    }

    class ServiceLoader : IServiceLoader {
        private static Dictionary<Type, object> Services = new Dictionary<Type, object>();
        private const string MISSING_SERVICE_LOG = "No service was registered for {0}.";

        internal void ClearServices() {
            if (Services == null) return;
            Services.Clear();
            Services = null;
        }

        internal void RegisterService(Type t, object service) {
            Services[t] = service;
        }

        internal void RegisterService<T>(T service) {
            Services[typeof(T)] = service;
        }

        public T GetService<T>() {
            var serviceType = typeof(T);
            if (Services.ContainsKey(serviceType))
                return (T)Services[serviceType];
            else
                throw new Exception(string.Format(MISSING_SERVICE_LOG, serviceType.Name));
        }

        internal bool CheckServiceRegistered<T>() {
            return Services.ContainsKey(typeof(T));
        }
    }   
}
