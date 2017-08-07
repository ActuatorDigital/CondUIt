using System;

namespace MVC {

    public interface IController {
        void Init(Action<IModel> context, IModel model);
        void Display();
        void LoadServices(IServicesLoader services);
        void LoadFramework(MVCFramework framework);
        bool Exclusive { get; }
    }
}