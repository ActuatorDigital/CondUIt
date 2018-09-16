using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using MVC;

public class CreateControllerEditorWindow : EditorWindow {

	string _controllerName = "";

	static int _selectedPopupIndex = 0;
	static List<Type> _modelTypes;

	public static void Display(){
		GetWindow<CreateControllerEditorWindow>("Add Controller");
	}

	 [UnityEditor.Callbacks.DidReloadScripts]
	private static void OnScriptsReloaded() {
		_modelTypes = EditorReflection.GetModelTypes();
	}

	void OnGUI(){

		EditorGUILayout.Popup(
			"Select Model",
			_selectedPopupIndex,
			_modelTypes.Select(m => m.FullName).ToArray()
		);	

		GUILayout.BeginHorizontal();
		GUILayout.Label("Enter name for controller", GUILayout.ExpandWidth(false));
		_controllerName = GUILayout.TextField(_controllerName, GUILayout.ExpandWidth(true) );
		GUILayout.EndHorizontal(); 

		var modelTypeStr = _modelTypes[_selectedPopupIndex].FullName;
		var classNameStr = String.IsNullOrEmpty(_controllerName) ? 
			modelTypeStr.Replace("Model", "") :
			 _controllerName;
		GUILayout.Label("public class " + classNameStr + "Controller : Controller<"+modelTypeStr+"> {");

		if(GUILayout.Button("Create")){
			
		}
		 
	}

}
