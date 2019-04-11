using System;
using System.IO;
using CondUIt;
using UnityEditor;
using UnityEngine;

public class CondUItEditorFactory {

    public static void AddControllerToScene(
        string className
    ){
        
        var typeStr = className+"Controller";
        var fullTypeStr = "Controllers."+typeStr;
        var type = GetTypeForControllerStr(fullTypeStr);
        if(type == null){
            Debug.LogError("Type " + typeStr + " is null");
            return;
        }

        var framework = GetFramework();
        ConfigureCanvas(framework);
        var controllerGo = new GameObject(typeStr);
        controllerGo.transform.SetParent(framework.gameObject.transform, true);

        controllerGo.AddComponent(type);
        var rt = controllerGo.GetComponent<RectTransform>();
        FullScaleRectTransform(rt);

        Selection.activeGameObject = controllerGo;
    }

    static void ConfigureCanvas(CondUItFramework framework)
    {
        var canvas = framework.GetComponent<Canvas>();
        var camera = GameObject.FindObjectOfType<Camera>();
        if(camera == null)
            camera = new GameObject("Main Camera").AddComponent<Camera>();
        
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = camera;
    }

    public static void AddViewToController(string controller, string view){

        var viewName = view + "View";
        var controllerType = GetTypeForControllerStr(controller);
        var controllerGo = GameObject.FindObjectOfType(controllerType);

        var viewGo = new GameObject(viewName);
        viewGo.transform.SetParent((controllerGo as Component).transform, true);

        var currentAssembly = EditorReflection.CurrentAssembly;
        var viewType = currentAssembly.GetType("Views."+viewName);
        viewGo.AddComponent(viewType);
        var rt = viewGo.GetComponent<RectTransform>();
        FullScaleRectTransform(rt);

        Selection.activeGameObject = viewGo;
    }
    
    public static void AddControllerToSolution(string className, string controllerCodeText)
    {
        var controllerFolder = "./Assets/Controllers";
        var fileName = SanatizeModelName(className) + "Controller.cs";
        GenerateFileForCode(controllerFolder, fileName, controllerCodeText);    
    }

    public static void AddModelToSolution(string modelName, string modelCodeText)
    {
        var modelFolder = "./Assets/Models";
        var fileName = SanatizeModelName(modelName) + "Model.cs";
        GenerateFileForCode(modelFolder, fileName, modelCodeText);
    }

    public static void AddViewToSolution(string viewName, string viewCodeText)
    {
        var modelFolder = "./Assets/Views";
        var fileName = SanatizeModelName(viewName) + "View.cs";
        GenerateFileForCode(modelFolder, fileName, viewCodeText);
    }

    static string SanatizeModelName(string model){
        return CondUItCodeGeneration.CamelCaseSentence(model);
    }

    static Type GetTypeForControllerStr(string controllerType){
        var currentAssembly = EditorReflection.CurrentAssembly;
        return currentAssembly.GetType(controllerType);
    }

    static void FullScaleRectTransform(RectTransform rt){
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.localScale = Vector2.one;
    }

    public static void GenerateFileForCode(
        string folder, 
        string fileName,
        string code
    ) {
        if(!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var codeFilePath = folder + "/" + fileName;
        if(!File.Exists(codeFilePath))
            File.WriteAllText(codeFilePath, code);

        AssetDatabase.Refresh();
        AssetDatabase.LoadAllAssetsAtPath(codeFilePath);
    }

    static CondUItFramework GetFramework(){

        var framework = UnityEngine.Object.FindObjectOfType<CondUItFramework>();
        if(!framework){
            framework = new GameObject("CondUItFramework")
                .AddComponent<CondUItFramework>();
        }

        return framework;

    }
    
}