using System;

namespace Conduit {

    public interface IController {
        void Display();
        void LoadServices(IServiceLoader services);
        void LoadFramework(ConduitUIFramework framework);
        bool Exclusive { get; }
        // void Init(IModel context);
    }

    public interface IFirstController : IController { }

}