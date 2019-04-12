using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public partial class CreateControllerEditorWindow : RecompileEditorWindow {

	int _selectedPopupIndex = 0;
	bool _exclusive = true;
    bool _modelBound = false;

    bool _controllerAddRequested = false;

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
        OnRecompileComplete += AddControllerToScene;        
	}

	void OnDisable(){
		OnRecompileComplete -= AddControllerToScene;
	}

	void AddControllerToScene(){
        if(!_controllerAddRequested) return;

		ConduitEditorFactory.AddControllerToScene(
			SelectedControllerStr );
        OnRecompileComplete -= AddControllerToScene;
        _controllerAddRequested = false;
	}

    private void DrawGenerateControllerButton(string controllerCode)
    {
        var keyPressed = Event.current.type == EventType.KeyDown;
        var enterPressed = keyPressed & Event.current.character == '\n';
        if (GUILayout.Button("Generate Controller") || enterPressed) {
            ConduitEditorFactory.AddControllerToSolution(
                SelectedControllerStr,
                controllerCode );
            GeneratingController = true;
    		_controllerAddRequested = true;
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

        var keyPressed = Event.current.type == EventType.KeyDown;
        var enterPressed = keyPressed & Event.current.character == '\n';
        if (GUILayout.Button("Generate Model") || enterPressed)
            CreateModelEditorWindow.Display();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private string DrawGeneratedText()
    {
		GUILayout.BeginVertical("Box");
        var isFirstController = FindObjectOfType<Conduit.FirstController>() == null;
        var generatedText = ConduitCodeGeneration
            .GenerateControllerTemplate(
                SelectedModelStr, 
                SelectedControllerStr,
                _exclusive,
                _modelBound,
                isFirstController
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
