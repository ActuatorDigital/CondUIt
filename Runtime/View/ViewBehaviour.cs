using System.Collections;
using UnityEngine;

namespace Conduit {

    public abstract class ViewBehaviour : MonoBehaviour {

        public void OnEnable() {
            if (ConduitServices.Services != null)
                LoadServices(ConduitServices.Services);
        }

        public abstract void LoadServices(IServiceLoader services);

    }

}
