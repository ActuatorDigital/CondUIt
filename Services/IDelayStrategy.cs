using System;

namespace Conduit
{
    public interface IDelayStrategy
    {
        void Process(Action callback, Action<float> progressHandler);
        void Process(Action callback, Action<float> progressHandler, TimeSpan delay);
        void Process(Action callback);
    }
}