using System;
using UnityEngine;

namespace AIR.Conduit {

    public class ConduitServices : IDisposable {

        private static ServiceLoader Services = new ServiceLoader();

        public ConduitServices RegisterService<T>(object service) {
            if (Services.CheckServiceRegistered<T>()) 
                Debug.LogWarning(
                        " A service for " + typeof(T).FullName +
                        " is already registered");
            else
                Services.RegisterService(typeof(T), service);
            return this;
        }

        public ConduitServices RegisterService<T>() where T : new() {
            var service = new T();
            Services.RegisterService(service);
            return this;
        }

        public static void DeployServices(ViewBehaviour viewBehaviour) {
            viewBehaviour.LoadServices(Services);
        }
        
        public static void DeployServices(IController controller) {
            controller.LoadServices(Services);
        }

        public void Dispose() {
            Services.ClearServices();
        }
    }

}
