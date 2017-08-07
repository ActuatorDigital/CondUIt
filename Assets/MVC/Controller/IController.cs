using System;

namespace MVC {

    public interface IController {
        void Init(Action<IModel> context, IModel model);
        void Display();
        void LoadServices(IServicesLoader services);
        bool Exclusive { get; }
    }
}