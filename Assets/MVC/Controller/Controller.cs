using MVC.Routing;
using MVC.Views;
using UnityEngine;

namespace MVC.Controllers
{
    public abstract class Controller : MonoBehaviour, IController
    {
        private IMvcRouting _routing;

        public void Init(IMvcRouting routing, IServicesLoader services)
        {
            _routing = routing;
            GetServices(services);
        }

        public void Action<C>(string action, params object[] args) where C : IController
        {
            _routing.Action<C>(action, args);
        }

        public void View<V>(object viewModel) where V : IView
        {
            _routing.View<V>(viewModel);
        }

        protected virtual void GetServices(IServicesLoader services) { }
    }
}