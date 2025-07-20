using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxyTerrariumController : MonoBehaviour
    {
        protected IEnumerator Start()
        {
            var item = GetComponentInParent<TerrariumItem>();

            TerrariumStateController state = null;
            while (state == null)
            {
                state = TerrariumController.Instance.GetStateController(item.StateData);
                if (state == null) yield return null;
            }

            var proxiedObjects = state.GetComponentsInChildren<ProxiedObjectController>();
            foreach(var proxiedObject in proxiedObjects)
            {
                var proxyObj = Instantiate(proxiedObject.ProxyPrefab);
                proxyObj.transform.SetParent(transform, false);
                var proxy = proxyObj.GetComponent<ProxyObjectController>();
                proxy.SetTarget(proxiedObject, state.transform, transform);
            }
        }
    }
}
