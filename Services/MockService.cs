using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Conduit {
    // NOTES: Maybe the idea of a mock service isn't actually useful and the mocking can be pushed into the data layer?
    //        That way we can still keep actual service behaviour while using mock data in testing. 
    //        Will look into some actual service implementations compared to mock implementations and see if this is possible.
    public abstract class MockService : MonoBehaviour {

        [SerializeField]
        private float _loadingDelay = 0.5f;

        //TODO: Subclasses use this method to allow external configuration at runtime.
        public virtual void OnDebugGUI() { }

        //TODO: This method is no longer needed provided we refactor out the Debug GUI and coroutines.
        public static T Create<T>() where T : MockService, new() {

            GameObject parentGO;

            var anyExistingService = FindObjectOfType<MockService>();
            if (anyExistingService.transform.parent == null) {
                parentGO = new GameObject("DebugServices");
            } else {
                parentGO = anyExistingService.transform.parent.gameObject;
            }

            var existingConcreteService = FindObjectOfType<T>();
            if (existingConcreteService == null) {
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(parentGO.transform);
                return go.AddComponent<T>();
            } else {
                existingConcreteService.transform.SetParent(parentGO.transform);
                return existingConcreteService;
            }
        }

        // This doesn't seem to be used in OnSite, surely this can be replaced by a conditional or even an overload of process that just doesn't have a progress handler parameter.
        public static Action<float> EmptyProgressHandler { get { return (p) => { }; } }

        protected void Process(Action callback, Action<float> progress) {
            StartCoroutine(ProcessRequest(callback, progress, _loadingDelay));
        }

        protected void Process(Action callback, Action<float> progress, float duration) {
            StartCoroutine(ProcessRequest(callback, progress, duration));
        }

        //TODO: Processing should be delegated to a collaborator so the mock service doesn't need to rely on Unity and can be implemented in the API
        private IEnumerator ProcessRequest(Action callback, Action<float> progressHandler, float duration) {
            float t = 0f;
            while (t < duration && duration > 0) {
                yield return null;
                t += Time.deltaTime;
                progressHandler.Invoke(t / duration);
            }

            callback.Invoke();
        }
    }

    //TODO: Refactor this into a debug config window or editor that allows you to configure all registered services?
    //      Alternatively make MockService an interface, derive an abstract subclass that also extends MonoBe
#if UNITY_EDITOR
    [CustomEditor(typeof(MockService), true)]
    public class DebugEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            var debugService = (MockService)target;
            debugService.OnDebugGUI();
        }
    }
#endif

}
