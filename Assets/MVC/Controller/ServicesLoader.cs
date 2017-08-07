using MVC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC {

    public class ServicesLoader : IServicesLoader {

        private Dictionary<Type, object> _services;
        private const string MISSING_SERVICE_LOG = "No service was registered for {0}.";

        /// <summary>
        /// Fill this method with RegisterService calls to populate the services map.
        /// </summary>
        private void PopulateServices() {
        }

        private void ClearServices() {
            _services.Clear();
            _services = null;
        }

        private IEnumerable<IController> FindControllers() {
            return ControllerReferences.Controllers.Select(c => c.GetComponent<IController>());
        }

        private void RegisterService<T>(T service) {
            _services[typeof(T)] = service;
        }

        public T GetService<T>() {
            var serviceType = typeof(T);
            if (_services.ContainsKey(serviceType))
                return (T)_services[serviceType];
            else
                throw new Exception(string.Format(MISSING_SERVICE_LOG, serviceType.Name));
        }

        public void DeliverServices() {
            _services = new Dictionary<Type, object>();
            PopulateServices();
            foreach (var c in FindControllers())
                c.LoadServices(this);
            ClearServices();
        }
    }

}
