using UnityEngine;

namespace AIR.Conduit {
    public abstract class TransitionHandler : MonoBehaviour {
        public abstract void OnShow();
        public abstract void OnHide();
    }
}