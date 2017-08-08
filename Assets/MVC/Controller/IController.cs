using System;

namespace MVC {

    public interface IController {
        void Init(IModel model);
        void Display();
        void LoadServices(IServicesLoader services);
        void LoadFramework(MVCFramework framework);
        bool Exclusive { get; }
    }
}