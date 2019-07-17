namespace Conduit {

    public interface IController {
        void Display();
        void LoadServices(IServiceLoader services);
        void LoadFramework(ConduitUIFramework framework);
        bool Exclusive { get; }
    }

    public interface IFirstController : IController { }

}