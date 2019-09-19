using System;

namespace Conduit {
    public interface IView {
        object Model { get; set; }
        bool HideNeighbours { get; }
        void Initialise(ConduitUIFramework framework);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}