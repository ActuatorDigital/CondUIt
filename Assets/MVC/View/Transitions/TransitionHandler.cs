using System;
using UnityEngine;

namespace MVC {
    public abstract class TransitionHandler : MonoBehaviour {
        public abstract void OnShow();
        public abstract void OnHide();
    }
}