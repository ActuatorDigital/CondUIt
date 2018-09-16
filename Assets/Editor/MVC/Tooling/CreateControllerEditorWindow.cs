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

	void OnGUI() {
		var generatedText = MvcCodeGeneration.GenerateControllerTemplate( SelectedModelStr, SelectedControllerStr );
		foreach(var line in generatedText.Split('\n'))
			GUILayout.Label(line);

		GUILayout.Space(20);

        _selectedPopupIndex = EditorGUILayout.Popup(
            "Select Model",
            _selectedPopupIndex,
            _modelTypes.Select(m => m.FullName).ToArray()
        );

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enter name for controller", GUILayout.ExpandWidth(false));
        _controllerName = GUILayout.TextField(_controllerName, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

		GUILayout.Space(10);

		if(!_modelTypes.Any()){

			GUILayout.BeginHorizontal();		
			GUILayout.FlexibleSpace();
			GUILayout.Label( "First add a model to solution.", GUILayout.ExpandWidth(true) );			
			GUILayout.FlexibleSpace();		
			GUILayout.EndHorizontal();

		} else if (!_generatingController) {

			if (GUILayout.Button("Create")) {
				MvcEditorFactory.AddControllerToSolution(
					SelectedControllerStr, 
					generatedText );
				_generatingController = true; 
			}

		} else {

			GUILayout.BeginHorizontal();		
			GUILayout.FlexibleSpace();
			GUILayout.Label( "Adding "+SelectedControllerStr+"Controller...", GUILayout.ExpandWidth(true) );			
			GUILayout.FlexibleSpace();		
			GUILayout.EndHorizontal();

		}

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
