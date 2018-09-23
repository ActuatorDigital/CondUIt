using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MvcCodeGeneration {
	public static string GenerateControllerTemplate(
		string modelTypeStr, 
		string classNameStr, 
		bool exclusive
	) {
		
		classNameStr = CamelCaseSentence(classNameStr);
		string generatedController = 
			"using MVC;\n\n" + 
			"namespace Controllers {\n" +
			"\tpublic class " + classNameStr + "Controller : Controller<" + modelTypeStr + "> {\n" +
			"\t\tpublic override bool Exclusive { get { return " + (exclusive ? "true" : "false")+ "; } }\n" +
			"\t\tpublic override void LoadServices(IServicesLoader services) { }\n" +
			"\t\tpublic override void Display() { }\n" +
			"\t}\n" +
			"}";

		return generatedController;
    }

    internal static string GenerateViewTemplate(
		string controllerName,
		string viewName
	) {

		string purpose = "";
		if( String.IsNullOrEmpty(viewName) ){
			if( !String.IsNullOrEmpty(controllerName) ){
				var controllerIndex = controllerName.IndexOf("Controller");
				purpose = controllerName.Substring(0, controllerIndex);
			} 
		} else 
			purpose = CamelCaseSentence(viewName);


		string viewModelName = purpose + "ViewModel";
		string generatedCode =  
			"using MVC;\n\n" + 
			"namespance Views {\n" + 
						"\tpublic class " + purpose + "View : View<" + viewModelName + ", " + controllerName + "> {\n" + 
			"\t\tprotected override void LoadElements() {}\n" + 
			"\t\tprotected override void ClearElements() {}\n" +
			"\t}\n"+
			"\tpublic struct " + viewModelName + "{ }\n" +
			"}";

		return generatedCode;
    }

    public static string GenerateModelTemplate(
		string modelName, 
		string modelParent,
		bool isModelRoot
	) {

		modelName = CamelCaseSentence(modelName);
		var modelExt = "Model" + 
			( isModelRoot ? "Root" : "" ) + 
			( isModelRoot ? "" : "<" + modelParent + ">" );
		var modelCodeStr = 
			"\n\n" +
		 	"using MVC;\n\n" +  
			"namespace Models {\n" + 
			"\tpublic class " + modelName + " : " + modelExt + " {\n\n" + 
			"\t}\n" + 
			"}\n\n";

        return modelCodeStr;
    }

	public static string CamelCaseSentence( string sentence ) {
		
		var words = sentence.Split(' ');
		var correctedWords = new List<string>();
		
		foreach(var word in words){
			var firstLetter = word.ToCharArray().FirstOrDefault();
			if( firstLetter == default(char)) continue;
			var remainingWord = word.Skip(1).Take(word.Length - 1);
			var camelCaseName = new char[] { char.ToUpper(firstLetter) }
				.Concat(remainingWord)
				.ToArray();
			var str = new String( camelCaseName );
			correctedWords.Add(str);
		}

		var camelCaseStr = String.Concat(correctedWords.ToArray());
		var rgx = new Regex("[^a-zA-Z0-9 -]");
		camelCaseStr = rgx.Replace(camelCaseStr, "");

		return camelCaseStr;
	}

}
