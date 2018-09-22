using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class CreateViewEditorWindow : EditorWindow {

	List<Type> 
        _modelTypes, 
        _controllerTypes;

    int _selectedModelIndex = 0;
    int _selectedControllerIndex = 0;

    public static void Display(){
        var window = GetWindow<CreateViewEditorWindow>("Add View");
        window.minSize = window.maxSize = new UnityEngine.Vector2(490, 250);
    }

    void OnGUI(){

        _selectedControllerIndex = EditorGUILayout.Popup(
            "Select Controller",
            _selectedControllerIndex,
            _controllerTypes.Select(m => m.FullName).ToArray()
        );

        _selectedModelIndex = EditorGUILayout.Popup(
            "Select Model",
            _selectedModelIndex,
            _modelTypes.Select(m => m.FullName).ToArray() );
            
    }

    void OnEnable(){
		_modelTypes = EditorReflection.GetModelTypes();
        _controllerTypes = EditorReflection.GetControllerTypes();
        EditorApplication.update += Update;
    }

    void OnDisable(){
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        //throw new NotImplementedException();
    }
}