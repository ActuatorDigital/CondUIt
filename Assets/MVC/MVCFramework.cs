using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVC {

    [RequireComponent(typeof(Canvas))]
    public class MVCFramework : MonoBehaviour{

        public List<MonoBehaviour> Views = new List<MonoBehaviour>();
        public List<MonoBehaviour> Controllers = new List<MonoBehaviour>();

        ServiceLoader _services = new ServiceLoader();

        public void Initialize<FirstController>() where FirstController : IController 
        {

            if (Controllers.Any())
                Controllers.Clear();
            if (Views.Any())
                Views.Clear();

            ConnectMVC();
            DeliverServices<FirstController>();
        }

        public MVCFramework RegisterService<T>(object service) {
            if (_services.CheckServiceRegistered<T>())
                throw new Exception("A service for " + typeof(T).FullName + " is already registered");
            _services.RegisterService(typeof(T), service);
            return this;
        }

        public MVCFramework RegisterService<T>() where T: new() {
            var service = new T();
            _services.RegisterService(service);
            return this;
        }

        void ConnectMVC()  {
            
            var behaviours = gameObject.GetComponentsInChildren(typeof(MonoBehaviour), true);
            foreach (MonoBehaviour b in behaviours) {
                if (b.GetType().GetInterfaces().Contains(typeof(IController))) 
                    Controllers.Add(b);
                if (b.GetType().GetInterfaces().Contains(typeof(IView)))
                    Views.Add(b);
            }

            HideViews();

        }
        
        internal void HideViews() {
            foreach (MonoBehaviour v in Views)
                v.gameObject.SetActive(false);
        }

        void DeliverServices<C>() where C : IController {
            foreach (IController c in Controllers) {
                c.LoadFramework(this);
                c.LoadServices(_services);
                if (c is C) {

                    var method = typeof(C).GetMethod("Main");
                    if (method == null)
                        throw new MissingMethodException(
                            "Main method not found on StartUp Controller" +
                            typeof(C).FullName + "." +
                            " Failed to Deliver Services.");

                    method.Invoke((c as IController), null);
                }
            }
            _services.ClearServices();
        }
    }

    class ServiceLoader : IServicesLoader {
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
