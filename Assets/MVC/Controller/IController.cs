using MVC.Routing;

namespace MVC.Controllers
{
    public interface IController
    {
        void Init(IMvcRouting routing, IServicesLoader services);
    }
}
