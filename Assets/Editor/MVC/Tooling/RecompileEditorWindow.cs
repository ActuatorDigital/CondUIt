
using System;
using UnityEditor;

public class RecompileEditorWindow : EditorWindow {

    protected static Action OnRecompileComplete { get; set; }
	protected bool GeneratingController = false;

    private void OnEnable() {
		EditorApplication.update += Update;
	}

	private void OnDisable() {
		EditorApplication.update -= Update;	
	}

    private void Update()
    {
		if(!GeneratingController) 
			return;
		else {
			if(!EditorApplication.isCompiling){
				GeneratingController = false;
				UnityEngine.Debug.Log("RecompileEditorWindow isCompiling");
				if(OnRecompileComplete != null){
					UnityEngine.Debug.Log("RecompileEditorWindow OnRecompileComplete");
					OnRecompileComplete();
				}else {
					UnityEngine.Debug.Log("OnRecomplileComplete null");
				}
			}

		}

    }
}