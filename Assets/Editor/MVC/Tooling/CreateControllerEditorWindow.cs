using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using MVC;
using System.IO;

public partial class CreateControllerEditorWindow : RecompileEditorWindow {

	int _selectedPopupIndex = 0;
	bool _exclusive = true;
    bool _modelBound = false;

    int _selectedModelIndex = 0;
    int _selectedControllerIndex = 0;

	string _controllerName = "";

	List<Type> _modelTypes;

	private string SelectedModelStr {
		get {
			var selectedModel = _modelTypes.Any() ? 
				_modelTypes[_selectedPopupIndex].Name :  
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
		var window = GetWindow<CreateControllerEditorWindow>(
            "Add Controller", true,
            typeof(CreateViewEditorWindow),
            typeof(CreateModelEditorWindow) );

		window.minSize = window.maxSize = new Vector2(490,250);

	}

	void OnEnable(){
		_modelTypes = EditorReflection.GetModelTypes();
        UnityEngine.Debug.Log("CreateControllerEditorWindow OnEnable");
	}

	void OnDisable(){
		OnRecompileComplete -= AddControllerToScene;
        UnityEngine.Debug.Log("CreateControllerEditorWindow OnDisable");
	}

    /// <summary>
    /// Adds newly compiled controller to scene.
    /// </summary>
	void AddControllerToScene(){
        UnityEngine.Debug.Log("CreateControllerEditorWindow AddControllerToScene");
		MvcEditorFactory.AddControllerToScene(
			// SelectedModelStr,
			SelectedControllerStr );
        OnRecompileComplete -= AddControllerToScene;
	}

    private void DrawGenerateControllerButton(string controllerCode)
    {
        if (GUILayout.Button("Generate Controller"))
        {
            MvcEditorFactory.AddControllerToSolution(
                SelectedControllerStr,
                controllerCode );
            GeneratingController = true;
    		OnRecompileComplete += AddControllerToScene;        
        }
    }


	void OnGUI()
    {
		
        GUILayout.Space(10);
        DrawInputRow();
        DrawNameInput();
		string controllerCode = DrawGeneratedText();
 
        GUILayout.Space(10);

        if (!_modelTypes.Any() && _modelBound)
            DrawCreateModelFirstMessage();
        else if (!GeneratingController)
            DrawGenerateControllerButton(controllerCode);
        else
            DrawGeneratingControllerMessage();

    }

	private void DrawInputRow(){
		GUILayout.BeginHorizontal();
		DrawModelInput();
		GUILayout.EndHorizontal();
	}

    private void DrawGeneratingControllerMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(
            "Adding " + SelectedControllerStr + "Controller...", 
            GUILayout.ExpandWidth(true) );
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private static void DrawCreateModelFirstMessage()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(
            "Generating a Controller, first requries a Model:", 
            GUILayout.ExpandWidth(true) );
		if(GUILayout.Button("Generate Model"))
            CreateModelEditorWindow.Display();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private string DrawGeneratedText()
    {
		GUILayout.BeginVertical("Box");
        var generatedText = MvcCodeGeneration
            .GenerateControllerTemplate(
                SelectedModelStr, 
                SelectedControllerStr,
                _exclusive,
                _modelBound
            );
        foreach (var line in generatedText.Split('\n'))
            GUILayout.Label(line.Replace("\t", "    "));
		GUILayout.EndVertical();
        return generatedText;
    }

    private void DrawModelInput()
    {
        if(_modelBound){
            _selectedPopupIndex = EditorGUILayout
                .Popup( "Select Model",
                        _selectedPopupIndex,
                        _modelTypes.Select(m => m.FullName).ToArray(),
                        GUILayout.ExpandWidth(true) );
        }
        _exclusive = GUILayout.Toggle(_exclusive, "Exclude others when activated.", GUILayout.ExpandWidth(true));
    }

    private void DrawNameInput()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Enter name for controller", GUILayout.ExpandWidth(false));
        _controllerName = GUILayout.TextField(_controllerName, GUILayout.ExpandWidth(true));
        _modelBound = GUILayout.Toggle(_modelBound, "Has Model", GUILayout.ExpandWidth(false));        
        GUILayout.EndHorizontal();
    }



}
