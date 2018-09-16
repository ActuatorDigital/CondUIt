using System.IO;
using MVC;
using UnityEditor;
using UnityEngine;

public class MvcEditorFactory {

    public static void AddControllerToScene(string modelName, string className){
        
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
    
    public static void AddControllerToSolution(string className, string generatedText){

        var controllerFolder = "./Assets/Controllers";
        if(!Directory.Exists(controllerFolder))
            Directory.CreateDirectory(controllerFolder);

        var controllerPath = controllerFolder + "/" + className + "Controller.cs";
        if(!File.Exists(controllerPath))
            File.WriteAllText(controllerPath, generatedText);

        AssetDatabase.Refresh();
        AssetDatabase.LoadAllAssetsAtPath(controllerPath);
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