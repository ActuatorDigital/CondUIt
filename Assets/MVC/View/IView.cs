
namespace MVC {
    public interface IView {

        object ViewModel { get; set; }

        void Render();
    }
}