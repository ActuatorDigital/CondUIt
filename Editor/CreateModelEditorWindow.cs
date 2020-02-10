using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AIR.Conduit.Editor {

    public partial class CreateModelEditorWindow : RecompileEditorWindow {

        List<Type> _modelTypes;
        int _selectedModelIndex = 0;
        string _modelName = "";
        bool _isModelRoot = false;

        string SelectedModelParent {
            get {
                var modelName = _selectedModelIndex > 0 ? _modelTypes[_selectedModelIndex].Name : "";
                return modelName;
            }
        }

        public static void Display() {
            var window = GetWindow<CreateModelEditorWindow>(
                "Add Model", true,
                typeof(CreateViewEditorWindow),
                typeof(CreateControllerEditorWindow));
            window.minSize = window.maxSize = new UnityEngine.Vector2(490, 250);
        }

        void OnEnable() {
            _modelTypes = EditorReflection.GetModelTypes();
        }

        void OnGUI() {

            GUILayout.Space(10);

            DrawModelNameInput();
            DrawModelParentSelection();

            string modelCode = DrawGeneratedText();

            DrawGenerateControllerButton(modelCode);

        }

        private void DrawModelParentSelection() {
            if (_modelTypes.Any()) {
                GUILayout.BeginHorizontal();
                if (!_isModelRoot) {
                    GUILayout.Label("Select Model Parent:", GUILayout.ExpandWidth(false));
                    _selectedModelIndex = EditorGUILayout.Popup(0, new String[] { }, GUILayout.ExpandWidth(true));
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawModelNameInput() {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Data description:", GUILayout.ExpandWidth(false));
            _modelName = GUILayout.TextField(_modelName, GUILayout.ExpandWidth(true));
            _isModelRoot =
                !_modelTypes.Any() ||
                GUILayout.Toggle(_isModelRoot, "Is Model Root", GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        private string DrawGeneratedText() {
            GUILayout.BeginVertical("Box");
            var generatedText = ConduitCodeGeneration
                .GenerateModelTemplate(
                    _modelName,
                    SelectedModelParent,
                    _isModelRoot);
            foreach (var line in generatedText.Split('\n'))
                GUILayout.Label(line.Replace("\t", "    "));
            GUILayout.EndVertical();

            return generatedText;
        }

        private void DrawGenerateControllerButton(string modelCode) {
            GUILayout.Space(10);
            if (GUILayout.Button("Generate Model")) {
                ConduitEditorFactory.AddModelToSolution(
                    _modelName,
                    modelCode);
                GeneratingController = true;
            }

            GUILayout.Space(10);
        }

    }
}