using System;

namespace MVC {
    public interface IView {
        object ViewModel { get; set; }
        bool IsPartial { get; }
        void Initialise(MVCFramework mvc);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}