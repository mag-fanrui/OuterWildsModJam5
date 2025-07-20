using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxyObjectController : MonoBehaviour
    {
        protected ProxiedObjectController target;

        public virtual void SetTarget(ProxiedObjectController target, Transform targetRoot, Transform selfRoot)
        {
            this.target = target;

            var relativePosition = targetRoot.InverseTransformPoint(target.transform.position);
            transform.position = selfRoot.TransformPoint(relativePosition);
            var relativeRotation = Quaternion.Inverse(targetRoot.rotation) * target.transform.rotation;
            transform.rotation = selfRoot.rotation * relativeRotation;
            transform.localScale = target.transform.localScale;
        }
    }
}
