using System;

namespace Conduit
{
    //TODO: Create a directory just for mock services?
    public interface IDelayStrategy
    {
        void Process(Action callback, Action<float> progressHandler, TimeSpan delay);
        void Process(Action callback, TimeSpan delay);
    }
}