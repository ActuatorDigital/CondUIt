using System;

namespace AIR.Conduit {
    public interface IView {
        object Model { get; set; }
        bool IsPartial { get; }
        void Initialise(ConduitUIFramework framework);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}