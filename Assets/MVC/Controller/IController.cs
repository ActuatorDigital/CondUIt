using System;

namespace MVC {

    public interface IController {
        void Display();
        void LoadServices(IServiceLoader services);
        void LoadFramework(MVCFramework framework);
        bool Exclusive { get; }
        // void Init(IModel context);
    }

}