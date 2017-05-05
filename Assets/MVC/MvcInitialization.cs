using MVC.Controllers;
using MVC.Routing;
using MVC.Views;
using UnityEngine;

namespace MVC
{
    public class MvcInitialization : MonoBehaviour
    {
        private void Awake()
        {
            var servicesLoader = new ServicesLoader();
            servicesLoader.Initialize();
            var routing = new MvcRouting();
            var behaviours = FindObjectsOfType<MonoBehaviour>();

            foreach(var b in behaviours)
            {
                var controller = b as IController;
                
                if(controller != null)
                {
                    routing.RegisterController(controller);
                    controller.Init(routing, servicesLoader);
                }

                var view = b as IView;

                if (view != null)
                {
                    routing.RegisterView(view);
                    view.Init(routing);
                    view.Hide();
                }
            }
        }
    }
}

