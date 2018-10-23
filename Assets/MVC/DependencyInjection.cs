using MVC;
using Services;
using UnityEngine;

[RequireComponent(typeof(MVCFramework))]
public class DependencyInjection : MonoBehaviour {

    [SerializeField] 
    private bool UseMockSystems = true;

	void Awake () {

#if !UNITY_EDITOR
        UseMockSystems = false;
#endif

        var framework = GetComponent<MVCFramework>();

        if (UseMockSystems) {            
            // Register mock services here.
        } else {
            // Register production services here.
        }

        // Initialize controller for the first UI here.
        // framework.Initialize<FirstController>();
    }

}
