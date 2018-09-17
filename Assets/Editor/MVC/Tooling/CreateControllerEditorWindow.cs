using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using MVC;
using System.IO;

public class CreateControllerEditorWindow : EditorWindow {

	string _controllerName = "";

	bool _generatingController = false;

	int _selectedPopupIndex = 0;
	List<Type> _modelTypes;
	bool __exclusive = false;

	private string SelectedModelStr {
		get {
			var selectedModel = _modelTypes.Any() ? 
				_modelTypes[_selectedPopupIndex].FullName : 
				string.Empty; 
			return selectedModel;
		}
	}

	private string SelectedControllerStr{
		get{
 			return String.IsNullOrEmpty(_controllerName) ?
				SelectedModelStr.Replace("Model", "") :
				_controllerName.Replace(" ", "");
		}
	}

	public static void Display() {
		var window = GetWindow<CreateControllerEditorWindow>("Add Controller");
		window.minSize = window.maxSize =new Vector2(490,250);
	}

	void OnGUI()
    {
		
        GUILayout.Space(10);

        DrawNameInput();
        DrawInputRow();
        
		string generatedText = DrawGeneratedText();
 
        GUILayout.Space(10);

        if (!_modelTypes.Any())
            DrawCreateModelFirstMessage();
        else if (!_generatingController)
            DrawGenerateControllerButton(generatedText);
        else
            DrawGeneratingControllerMessage();

    }

	private void DrawInputRow(){
		GUILayout.BeginHorizontal();
		DrawModelInput();
		DrawExclusiveInput();
		GUILayout.EndHorizontal();
	}

    private void DrawExclusiveInput()
    {
        __exclusive = GUILayout.Toggle(__exclusive, "Exclude others when activated.");
    }

    private void DrawGeneratingControllerMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Adding " + SelectedControllerStr + "Controller...", GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawGenerateControllerButton(string generatedText)
    {
        if (GUILayout.Button("Create"))
        {
            MvcEditorFactory.AddControllerToSolution(
                SelectedControllerStr,
                generatedText);
            _generatingController = true;
        }
    }

    private static void DrawCreateModelFirstMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Generating a Cotnroller, first requried a Model:", GUILayout.ExpandWidth(true));
		GUILayout.Button("Generate Model");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private string DrawGeneratedText()
    {
		GUILayout.BeginVertical("Box");
        var generatedText = MvcCodeGeneration.GenerateControllerTemplate(SelectedModelStr, SelectedControllerStr);
        foreach (var line in generatedText.Split('\n'))
            GUILayout.Label(line);
		GUILayout.EndVertical();
        return generatedText;
    }

    private void DrawModelInput()
    {
        _selectedPopupIndex = EditorGUILayout.Popup(
                    "Select Model",
                    _selectedPopupIndex,
                    _modelTypes.Select(m => m.FullName).ToArray()
                );
    }

    private void DrawNameInput()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Enter name for controller", GUILayout.ExpandWidth(false));
        _controllerName = GUILayout.TextField(_controllerName, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
    }

    private void OnEnable() {
		_modelTypes = EditorReflection.GetModelTypes();
		EditorApplication.update += Update;
	}

	private void OnDisable() {
		EditorApplication.update -= Update;	
	}

	void Update(){

		if(!_generatingController && !EditorApplication.isCompiling) return;

		if(!EditorApplication.isCompiling) _generatingController = false;

		if(!EditorApplication.isCompiling)
			MvcEditorFactory.AddControllerToScene(SelectedModelStr, SelectedControllerStr);

	}


}
