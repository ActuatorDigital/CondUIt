using AIR.Conduit;
using UnityEngine;
using UnityEngine.Events;

namespace AIR.Conduit {
    public class TransitionEventTrigger : TransitionHandler {
        [SerializeField]
        private UnityEvent _showHandlers = null;
        [SerializeField]
        private UnityEvent _hideHandlers = null;

        public override void OnHide() {
            _hideHandlers.Invoke();
        }

        public override void OnShow() {
            _showHandlers.Invoke();
        }
    }
}

