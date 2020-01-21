using System;
using UnityEngine;

namespace Conduit {

    public class ConduitServices : MonoBehaviour {

        public static ServiceLoader Services = new ServiceLoader();

        public ConduitServices RegisterService<T>(object service) {
            if (Services.CheckServiceRegistered<T>())
                throw new Exception("Services should be Singletons." +
                    " A service for " + typeof(T).FullName +
                    " is already registered");
            Services.RegisterService(typeof(T), service);
            return this;
        }

        public ConduitServices RegisterService<T>() where T : new() {
            var service = new T();
            Services.RegisterService(service);
            return this;
        }

    }

}
