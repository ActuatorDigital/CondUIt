using System;
using System.IO;
using MVC;
using UnityEditor;
using UnityEngine;

public class MvcEditorFactory {

    public static void AddControllerToScene(
        // string modelName, 
        string className
    ){
        
        // UnityEngine.Debug.Log("Create a new controller " + className + " bound to model " + modelName);

        // var currentAssembly = EditorReflection.CurrentAssembly;
        // var typeName = "Controllers."+className+"Controller";
        // var type = currentAssembly.GetType(typeName);
        var typeStr = className+"Controller";
        var fullTypeStr = "Controllers."+typeStr;
        var type = GetTypeForControllerStr(fullTypeStr);
        if(type == null){
            Debug.LogError("Type " + typeStr + " is null");
            return;
        }

        // string fullClassName = typeStr;
        var framework = GetFramework();
        var controllerGo = new GameObject(typeStr);
        controllerGo.transform.parent = framework.gameObject.transform;

        controllerGo.AddComponent(type);
    }

    public static void AddViewToController(string controller, string view){
        UnityEngine.Debug.Log("Add View with parent controller " + controller + " view " + view );

        var viewName = view + "View";
        var controllerType = GetTypeForControllerStr(controller);
        var controllerGo = GameObject.FindObjectOfType(controllerType);

        var viewGo = new GameObject(viewName);
        viewGo.transform.parent = (controllerGo as Component).transform;

        var currentAssembly = EditorReflection.CurrentAssembly;
        var viewType = currentAssembly.GetType("Views."+viewName);
        viewGo.AddComponent(viewType);
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
        UnityEngine.Debug.Log("AddViewToSolution");
    }

    static string SanatizeModelName(string model){
        return MvcCodeGeneration.CamelCaseSentence(model);
    }

    static Type GetTypeForControllerStr(string controllerType){
        var currentAssembly = EditorReflection.CurrentAssembly;
        return currentAssembly.GetType(controllerType);
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

    static MVCFramework GetFramework(){

        var framework = UnityEngine.Object.FindObjectOfType<MVCFramework>();
        if(!framework){
            framework = new GameObject("MVCFramework")
                .AddComponent<MVCFramework>();
        }

        return framework;

    }
    
}