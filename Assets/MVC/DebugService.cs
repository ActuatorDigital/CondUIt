using System;
using System.Collections;
using UnityEngine;

public abstract class DebugService : MonoBehaviour {

    [SerializeField]
    private float _loadingDelay = 0.5f;

    public static DebugService Create<T> () where T : DebugService, new() 
    {
        var existingService = FindObjectOfType<DebugService>();
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
