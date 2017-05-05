using MVC.Routing;

namespace MVC.Views
{
    public interface IView
    {
        void Init(IMvcRouting routing);
        void Render(object model);
        void Hide();
    }
}

