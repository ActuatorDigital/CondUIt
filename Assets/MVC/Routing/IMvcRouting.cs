using MVC.Controllers;
using MVC.Views;

namespace MVC.Routing
{
    public interface IMvcRouting
    {
        void View<V>(object model) where V : IView;
        void Action<C>(string action, params object[] args) 
            where C : IController;
    }
}
