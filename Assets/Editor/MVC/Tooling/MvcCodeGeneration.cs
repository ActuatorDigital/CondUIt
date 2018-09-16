using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MvcCodeGeneration {
	public static string GenerateControllerTemplate(string modelTypeStr, string classNameStr) {
		
		string generatedClass = 
			"using MVC;\n\n" + 
			"namespace Controllers {\n" +
			"\tpublic class " + classNameStr + "Controller : Controller<" + modelTypeStr + "> {\n" +
			"\t\tpublic override bool Exclusive { get { return true; } }\n" +
			"\t\tpublic override void LoadServices(IServicesLoader services) { }\n" +
			"\t\t//public override void Display() { }\n" +
			"\t}\n" +
			"}";

		return generatedClass;
    }
}
