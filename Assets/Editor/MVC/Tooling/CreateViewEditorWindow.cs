using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class CreateViewEditorWindow : RecompileEditorWindow {

	List<Type> _controllerTypes;

    string _viewName = "";
    int _selectedControllerIndex = 0;
    bool _generateViewModel = true;

    string SelectedControllerName {
        get { 
            string selectedController = _controllerTypes.Any() ? 
                _controllerTypes[_selectedControllerIndex].Name : "";
            return selectedController;
        }
    }
    
    public static void Display() 
    {
        UnityEngine.Debug.Log("CreateViewEditorWindow");
        var window = GetWindow<CreateViewEditorWindow>(
            "Add View", true,
            typeof(CreateControllerEditorWindow),
            typeof(CreateViewEditorWindow) );
        window.minSize = window.maxSize = new UnityEngine.Vector2(490, 250);
    }
    
    void OnEnable() 
    {
        _controllerTypes = EditorReflection.GetControllerTypes();
        OnRecompileComplete += AddViewToController;
    }

    void OnDisable(){
        OnRecompileComplete -= AddViewToController;
    }

    void AddViewToController() 
    {
        var controller = _controllerTypes[_selectedControllerIndex];
        UnityEngine.Debug.Log("Add View with parent controller " + controller);
    }

#region UI
    void OnGUI()
    {

        GUILayout.Space(13);

        DrawControllerSelectDropdown();
        DrawViewNameInputField();
        var viewCode = DrawGeneratedText();

        GUILayout.Space(8);

        if(!_controllerTypes.Any())
            DrawCreateControllerFirstMessage();
        else if (!GeneratingController) 
            DrawGenerateViewButton(viewCode); 
        else
            DrawGeneratingViewMessage();

    }

    private void DrawViewNameInputField() 
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("View's Function:", GUILayout.ExpandWidth(false));
        _viewName = GUILayout.TextField(_viewName, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
    }

    private void DrawGeneratingViewMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Adding " + SelectedControllerName + " view . . .", GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawCreateControllerFirstMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(
            "Generating a View, first requires a Controller:", 
            GUILayout.ExpandWidth(true) );
		if(GUILayout.Button("Generate Controller"))
            CreateControllerEditorWindow.Display();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawControllerSelectDropdown()
    {
        _selectedControllerIndex = EditorGUILayout.Popup(
            "Select Controller",
            _selectedControllerIndex,
            _controllerTypes.Select(m => m.FullName).ToArray()
        );
    }

    string DrawGeneratedText()
    {
		GUILayout.BeginVertical("Box");
        var generatedViewCode = MvcCodeGeneration
            .GenerateViewTemplate( 
                SelectedControllerName,
                _viewName );
        foreach (var line in generatedViewCode.Split('\n'))
            GUILayout.Label(line.Replace("\t", "     "));
        GUILayout.EndVertical();
        return generatedViewCode;
    }

    private void DrawGenerateViewButton(string viewCode) 
    {
        if(GUILayout.Button("Generate View"))
            UnityEngine.Debug.Log("Generate View!");
    }

#endregion

}