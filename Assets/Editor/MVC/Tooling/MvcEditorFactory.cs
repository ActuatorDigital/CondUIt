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

        var currentAssembly = EditorReflection.CurrentAssembly;
        var typeName = "Controllers."+className+"Controller";
        var type = currentAssembly.GetType(typeName);
        if(type == null){
            Debug.LogError("Type is null");
            return;
        }

        string fullClassName = className+"Controller";
        var framework = GetFramework();
        var controllerGo = new GameObject(fullClassName);
        controllerGo.transform.parent = framework.gameObject.transform;

        controllerGo.AddComponent(type);
    }

    public static void AddViewToController(string controller, string view){
        Debug.Log("AddViewToController");
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
        return MvcCodeGeneration.CamelCaseSentence(model);
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

        var framework = Object.FindObjectOfType<MVCFramework>();
        if(!framework){
            framework = new GameObject("MVCFramework")
                .AddComponent<MVCFramework>();
        }

        return framework;

    }
    
}