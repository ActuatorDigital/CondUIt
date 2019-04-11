using System;

namespace CondUIt {

    public interface IController {
        void Display();
        void LoadServices(IServiceLoader services);
        void LoadFramework(CondUItFramework framework);
        bool Exclusive { get; }
        // void Init(IModel context);
    }

}