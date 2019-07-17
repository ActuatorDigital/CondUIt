using UnityEngine;
using UnityEditor;

public class ConduitContextMenus : MonoBehaviour {

	[MenuItem("GameObject/UI/Conduit/Controller")]
	static void CreateController(){
		CreateControllerEditorWindow.Display();
	}

	[MenuItem("GameObject/UI/Conduit/View")]
	static void CreateView(){
		CreateViewEditorWindow.Display();
	}


	[MenuItem("GameObject/UI/Conduit/Model")]
	static void CreateModel(){
		CreateModelEditorWindow.Display();
	}

}