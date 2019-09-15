using System;
using System.Collections.Generic;

namespace Conduit {
    class ServiceLoader : IServiceLoader {
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private const string MISSING_SERVICE_LOG = "No service was registered for {0}.";

        internal void ClearServices() {
            if (_services == null) return;
            _services.Clear();
            _services = null;
        }

        internal void RegisterService(Type t, object service) {
            _services[t] = service;
        }

        internal void RegisterService<T>(T service) {
            _services[typeof(T)] = service;
        }

        public T UseService<T>() {
            var serviceType = typeof(T);
            if (_services.ContainsKey(serviceType))
                return (T)_services[serviceType];
            else
                throw new Exception(string.Format(MISSING_SERVICE_LOG, serviceType.Name));
        }

        internal bool CheckServiceRegistered<T>() {
            return _services.ContainsKey(typeof(T));
        }
    }
}