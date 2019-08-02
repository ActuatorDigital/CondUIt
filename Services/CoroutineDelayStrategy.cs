using System;
using System.Collections;
using UnityEngine;

namespace Conduit
{
    public class CoroutineDelayStrategy : MonoBehaviour, IDelayStrategy
    {
        [SerializeField, Range(0, 10f)]
        private float _secondsToDelay = 0.5f;

        public void Process(Action callback, Action<float> progressHandler)
        {
            Process(callback, progressHandler, TimeSpan.FromSeconds(_secondsToDelay));
        }

        public void Process(Action callback)
        {
            Process(callback, NullProgressHandler);
        }

        public void Process(Action callback, Action<float> progressHandler, TimeSpan delay)
        {
            StartCoroutine(ProcessCallback(callback, progressHandler, delay));
        }

        private IEnumerator ProcessCallback (Action callback, Action<float> progressHandler, TimeSpan delay)
        {
            float t = 0f, secondsToDelay = (float)delay.TotalSeconds;
            while (t < secondsToDelay && secondsToDelay > 0)
            {
                yield return null;
                t += Time.deltaTime;
                progressHandler.Invoke(t / secondsToDelay);
            }

            callback.Invoke();
        }

        private void NullProgressHandler(float progress)
        {
        }
    }
}
