
namespace MVC {
    public interface IView {

        object ViewModel { get; set; }

        bool IsPartial { get; }

        void Render();
    }
}