using System;

namespace MVC {
    public interface IView {
        object Model { get; set; }
        bool IsPartial { get; }
        void Initialise(MVCFramework mvc);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}