using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Services {
    public abstract class MockService : MonoBehaviour {

        [SerializeField]
        private float _loadingDelay = 0.5f;

        public virtual void OnDebugGUI(){}

        public static T Create<T> () where T : MockService, new() 
        {
            var existingService = FindObjectOfType<MockService>();
            var parentGO = existingService == null ? new GameObject("Debug Services") : 
                        existingService.transform.parent.gameObject;

            var go = new GameObject(typeof(T).Name);
            go.transform.SetParent(parentGO.transform);
            return go.AddComponent<T>();
        }

        public static Action<float> EmptyProgressHandler { get { return (p) => { }; } }

        protected void Process(Action callback, Action<float> progress) {
            StartCoroutine(ProcessRequest(callback, progress, _loadingDelay));
        }

        protected void Process(Action callback, Action<float> progress, float duration)
        {
            StartCoroutine(ProcessRequest(callback, progress, duration));
        }

        private IEnumerator ProcessRequest (Action callback, Action<float> progressHandler, float duration)
        {
            float t = 0f;
            while(t < duration && duration > 0)
            {
                yield return null;
                t += Time.deltaTime;
                progressHandler.Invoke(t / duration);
            }

            callback.Invoke();
        }
    }

    
#if UNITY_EDITOR
    [CustomEditor(typeof(MockService), true)]
    public class DebugEditor : Editor {
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            var debugService = (MockService)target;
            debugService.OnDebugGUI();
        }
    }
#endif

}
