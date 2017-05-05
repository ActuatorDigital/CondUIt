namespace MVC.Views
{
    public interface IView
    {
        object ViewModel { get; set; }
        void Render();
    }
}

