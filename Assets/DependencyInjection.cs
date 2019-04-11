using UnityEngine;
using Conduit;

[RequireComponent(typeof(ConduitUIFramework))]
public class DependencyInjection : MonoBehaviour {

    [SerializeField] 
    private bool UseMockSystems = true;

	void Awake () {

#if !UNITY_EDITOR
        UseMockSystems = false;
#endif

        var services = GetComponent<ConduitServices>();

        if (UseMockSystems) {            
            // Register mock services here.
        } else {
            // Register production services here.
        }

    }

}
