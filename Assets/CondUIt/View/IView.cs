using System;

namespace Conduit {
    public interface IView {
        dynamic ViewModel { get; set; }
        ViewBinding ViewBinding { get; set; }
        bool IsPartial { get; }
        void Initialise(ConduitUIFramework framework);
        void Render();
        void Hide();
        Type GetControllerType();
    }
}