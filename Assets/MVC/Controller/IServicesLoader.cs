namespace MVC
{
    public interface IServicesLoader
    {
        T GetService<T>();
    }
}

