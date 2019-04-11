using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CondUItContextMenus : MonoBehaviour {

	[MenuItem("GameObject/UI/CondUIt/Controller")]
	static void CreateController(){
		CreateControllerEditorWindow.Display();
	}

	[MenuItem("GameObject/UI/CondUIt/View")]
	static void CreateView(){
		CreateViewEditorWindow.Display();
	}


	[MenuItem("GameObject/UI/CondUIt/Model")]
	static void CreateModel(){
		CreateModelEditorWindow.Display();
	}

}