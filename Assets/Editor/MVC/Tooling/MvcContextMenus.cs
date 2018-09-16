using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MvcContextMenus : MonoBehaviour {

	[MenuItem("GameObject/UI/MVC/Controller")]
	static void CreateController(){
		CreateControllerEditorWindow.Display();
	}

	[MenuItem("GameObject/UI/MVC/View")]
	static void CreateView(){
		UnityEngine.Debug.Log("Show View Creation menu here.");
	}


	[MenuItem("GameObject/UI/MVC/Model")]
	static void CreateModel(){
		UnityEngine.Debug.Log("Show Model Creation menu here.");
	}

}