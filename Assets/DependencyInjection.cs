using UnityEngine;
using CondUIt;

[RequireComponent(typeof(CondUItFramework))]
public class DependencyInjection : MonoBehaviour {

    [SerializeField] 
    private bool UseMockSystems = true;

	void Awake () {

#if !UNITY_EDITOR
        UseMockSystems = false;
#endif

        var framework = GetComponent<CondUItFramework>();

        if (UseMockSystems) {            
            // Register mock services here.
        } else {
            // Register production services here.
        }

        // Initialize controller for the first UI here.
        //framework.InitializeUI<FirstController>();
    }

}
