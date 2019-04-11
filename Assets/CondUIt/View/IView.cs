using System;

namespace CondUIt {
    public interface IView {
        object Model { get; set; }
        bool IsPartial { get; }
        void Initialise(CondUItFramework framework);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}