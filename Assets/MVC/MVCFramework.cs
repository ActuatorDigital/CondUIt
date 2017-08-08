﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC {

    public class MVCFramework : MonoBehaviour{

        public List<MonoBehaviour> Views = new List<MonoBehaviour>();
        public List<MonoBehaviour> Controllers = new List<MonoBehaviour>();
        public Action<IModel> OnSaveChanges;

        ServiceLoader _services = new ServiceLoader();

        public void Initialize<C>( Action<IModel> onSave, IModel model ) 
                where C : IController {

            OnSaveChanges = onSave;

            if (Controllers.Any())
                Controllers.Clear();
            if (Views.Any())
                Views.Clear();

            ConnectMVC();
            DeliverServices<C>(model);
        }

        void ConnectMVC()  {

            var behaviours = gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (var b in behaviours) {
                if (b.GetType().GetInterfaces().Contains(typeof(IController))) {
                    Controllers.Add(b);

                } if (b.GetType().GetInterfaces().Contains(typeof(IView)))
                    Views.Add(b);
            }

            HideViews();

        }

        internal void HideViews() {
            foreach (MonoBehaviour v in Views)
                v.gameObject.SetActive(false);
        }

        void DeliverServices<C>(IModel model) where C : IController {
            foreach (IController c in Controllers) {
                c.LoadFramework(this);
                c.LoadServices(_services);
                if (c is C) {
                    var firstController = (c as IController);
                    firstController.Init(model);
                    firstController.Display();
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
    
}