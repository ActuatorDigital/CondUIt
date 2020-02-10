using System.Collections;
using UnityEngine;

namespace AIR.Conduit {

    public abstract class ViewBehaviour : MonoBehaviour {
        
        public void OnEnable() {
            ConduitServices.DeployServices(this);
        }

        public abstract void LoadServices(IServiceLoader services);

    }

}
