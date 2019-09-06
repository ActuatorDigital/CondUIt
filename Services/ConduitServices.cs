using System;
using UnityEngine;

namespace Conduit {

    public class ConduitServices : MonoBehaviour {

        internal ServiceLoader _services = new ServiceLoader();

        public ConduitServices RegisterService<T>(T service) {
            if (_services.CheckServiceRegistered<T>())
                throw new Exception("Services should be Singletons." +
                    " A service for " + typeof(T).FullName +
                    " is already registered");
            _services.RegisterService(typeof(T), service);
            return this;
        }

        public ConduitServices RegisterService<T>() where T : new() {
            var service = new T();
            _services.RegisterService(service);
            return this;
        }

    }

}
