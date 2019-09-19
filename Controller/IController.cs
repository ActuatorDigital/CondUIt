namespace Conduit {

    /// <summary>
    /// Controllers connect the injected service layer to the view layer.
    /// They handle the movement through UI areas via routing, and deliver
    /// hold logic for retrieving and reforming data for use in views.
    /// </summary>
    public interface IController {
        
        /// <summary>
        /// Called after routing to this controller.
        /// </summary>
        void Routed();

        /// <summary>
        /// Enables the loading of services registered during dependency injecton.
        /// </summary>
        /// <param name="services"></param>
        void LoadServices(IServiceLoader services);

        void LoadFramework(ConduitUIFramework framework);
    }

    public interface IInitialController : IController { }

}