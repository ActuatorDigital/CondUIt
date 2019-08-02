using System;

namespace Conduit
{
    public abstract class MockService
    {
        private readonly IDelayStrategy _delayStrategy;

        protected MockService(IDelayStrategy delayStrategy)
        {
            _delayStrategy = delayStrategy;
        }

        protected void Process(Action callback, Action<float> progress)
        {
            _delayStrategy.Process(callback, progress);
        }

        protected void Process(Action callback, Action<float> progress, float duration)
        {
            _delayStrategy.Process(callback, progress, TimeSpan.FromSeconds(duration));
        }
    }
}
