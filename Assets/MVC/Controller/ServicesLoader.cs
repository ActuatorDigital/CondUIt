using MVC.Example;
using System;
using System.Collections.Generic;

namespace MVC
{
    public class ServicesLoader : IServicesLoader
    {
        private Dictionary<Type, object> _services;
        private const string MISSING_SERVICE_LOG = "No service was registered for {0}.";

        private void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public void Initialize ()
        {
            _services = new Dictionary<Type, object>();
            var team = new TeamModel();
            RegisterService<ITeamServices>(new TeamServices(team));
            RegisterService<IMonsterService>(new MonsterService(team));
        }

        public T GetService<T>()
        {
            var serviceType = typeof(T);
            if (_services.ContainsKey(serviceType))
                return (T)_services[serviceType];
            else
                throw new Exception(string.Format(MISSING_SERVICE_LOG, serviceType.Name));
        }
    }
}

