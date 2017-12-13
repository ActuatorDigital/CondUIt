using System;

namespace MVC {

    public interface IController {
        void LoadServices(IServicesLoader services);
        void LoadFramework(MVCFramework framework);
        bool Exclusive { get; }
    }

}