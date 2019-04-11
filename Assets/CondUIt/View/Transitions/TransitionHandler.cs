using System;
using UnityEngine;

namespace Conduit {
    public abstract class TransitionHandler : MonoBehaviour {
        public abstract void OnShow();
        public abstract void OnHide();
    }
}