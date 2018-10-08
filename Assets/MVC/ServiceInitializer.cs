using MVC;
using Services;
using UnityEngine;

public class ServiceInitializer : MonoBehaviour {

    [SerializeField] 
    private bool UseMockSystems = true;

	void Awake () {

#if !UNITY_EDITOR
        UseMockSystems = false;
#endif

        var framework = FindObjectOfType<MVCFramework>();

        if (UseMockSystems) {            
        } else {
        }


        // framework.Initialize<FirstController>();
    }

}
