
using System;
using System.Collections;

namespace Conduit {
    public interface IControllerLoader: IEnumerable {
        C UseController<C>() where C : IController;
        IController UseController(Type type);
        void Register(IController[] controller);
        IInitialController GetInitialController();
    }
}