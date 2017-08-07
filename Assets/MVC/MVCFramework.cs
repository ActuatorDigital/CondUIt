using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC {

    public class MVCFramework : MonoBehaviour{

        public List<MonoBehaviour> Views = new List<MonoBehaviour>();
        public List<MonoBehaviour> Controllers = new List<MonoBehaviour>();

        public ServiceLoader _services = new ServiceLoader();

        private void Start() {
            Initialize();
        }

        public void Initialize() {

            if (Controllers.Any())
                Controllers.Clear();
            if (Views.Any())
                Views.Clear();

            ConnectMVC();
            DeliverServices();
        }

        void ConnectMVC() {

            var behaviours = gameObject.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (var b in behaviours) {
                if (b.GetType().GetInterfaces().Contains(typeof(IController)))
                    Controllers.Add(b);
                if (b.GetType().GetInterfaces().Contains(typeof(IView)))
                    Views.Add(b);
            }

            HideViews();

        }

        public void HideViews() {
            foreach (MonoBehaviour v in Views)
                v.gameObject.SetActive(false);
        }

        public void DeliverServices() {
            foreach (IController c in Controllers) {
                c.LoadFramework(this);
                c.LoadServices(_services);
            }
            _services.ClearServices();
        }
    }

    public class ServiceLoader : IServicesLoader {
        private static Dictionary<Type, object> Services = new Dictionary<Type, object>();
        private const string MISSING_SERVICE_LOG = "No service was registered for {0}.";

        internal void ClearServices() {
            if (Services == null) return;
            Services.Clear();
            Services = null;
        }

        private void RegisterService<T>(T service) {
            Services[typeof(T)] = service;
        }

        public T GetService<T>() {
            var serviceType = typeof(T);
            if (Services.ContainsKey(serviceType))
                return (T)Services[serviceType];
            else
                throw new Exception(string.Format(MISSING_SERVICE_LOG, serviceType.Name));
        }

    }

    public static class GameObjectExtensions {

        public static List<T> FindObjectsOfTypeAll<T>() {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded) {
                    var allGameObjects = s.GetRootGameObjects();
                    for (int j = 0; j < allGameObjects.Length; j++) {
                        var go = allGameObjects[j];
                        results.AddRange(go.GetComponentsInChildren<T>(true));
                    }
                }
            }
            return results;
        }

        public static List<T> FindObjectsOfTypeAll<T>(this GameObject mb) {
            return FindObjectsOfTypeAll<T>();
        }

        public static IController FindControllerOfType(this GameObject mb, Type contextType) {
            var controllerType = typeof(Controller<>).MakeGenericType(contextType);
            var controller = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour))
                .FirstOrDefault(c => c.GetType().IsSubclassOf(controllerType))
                    as IController;
            return controller;
        }

    }
    
}
