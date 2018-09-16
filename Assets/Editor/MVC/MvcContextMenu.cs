using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MvcContextMenu : MonoBehaviour {

	[MenuItem("GameObject/UI/MVC/Controller")]
	static void CreateController(){
		UnityEngine.Debug.Log("Show Controller creation menu here");
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