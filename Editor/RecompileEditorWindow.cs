
using System;
using UnityEditor;

namespace AIR.Conduit.Editor {
    public class RecompileEditorWindow : EditorWindow {

        protected static Action OnRecompileComplete { get; set; }
        protected bool GeneratingController = false;

        private void OnEnable() {
            EditorApplication.update += Update;
        }

        private void OnDisable() {
            EditorApplication.update -= Update;
        }

        private void Update() {
            if (!GeneratingController)
                return;
            else {
                if (!EditorApplication.isCompiling) {
                    GeneratingController = false;
                    if (OnRecompileComplete != null) {
                        OnRecompileComplete();
                    }
                }

            }

        }
    }
}