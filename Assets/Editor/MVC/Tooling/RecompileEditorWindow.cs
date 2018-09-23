
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
		if(!GeneratingController && !EditorApplication.isCompiling) return;

		if(!EditorApplication.isCompiling) GeneratingController = false;

		if(!EditorApplication.isCompiling)
            if(OnRecompileComplete != null)
			    OnRecompileComplete();
    }
}