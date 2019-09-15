
using System;
using System.Collections;

namespace Conduit {
    public interface IControllerLoader: IEnumerable {
        C LoadController<C>() where C : IController;
        IController LoadController(Type type);
        void Register(IController[] controller);
        IInitialController GetInitialController();
    }
}