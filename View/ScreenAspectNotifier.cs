using System;
using System.Collections;
using UnityEngine;

namespace Conduit {

    public enum ScreenAspect {
        Portrait = 0,
        Landscape = 1
    }

    public class ScreenAspectNotifier : MonoBehaviour {
        
        static ScreenAspect _screenOrientation = ScreenAspect.Landscape;

        public static Action<ScreenAspect> OnScreenOrientationChanged { get; set; }
        public static ScreenAspect CurrentScreenAspect { get { return _screenOrientation; } }

        private void Start() {
            StartCoroutine(RespondToScreenOrientation());
        }

        private IEnumerator RespondToScreenOrientation() {
            while (true) {
                yield return new WaitForEndOfFrame();
                var lanscapeRes = Screen.width > Screen.height;
                var isLandscapeOriantation =
                    _screenOrientation == ScreenAspect.Landscape;
                if (lanscapeRes && !isLandscapeOriantation) {
                    _screenOrientation = ScreenAspect.Landscape;
                    OnScreenOrientationChanged?.Invoke(_screenOrientation);
                } else if (!lanscapeRes && isLandscapeOriantation) {
                    _screenOrientation = ScreenAspect.Portrait;
                    OnScreenOrientationChanged?.Invoke(_screenOrientation);
                }
            }
        }

    }
}