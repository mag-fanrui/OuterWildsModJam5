using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxyPlantController : ProxyObjectController
    {
        protected PlantController targetPlant;

        public override void SetTarget(ProxiedObjectController target, Transform targetRoot, Transform selfRoot)
        {
            base.SetTarget(target, targetRoot, selfRoot);

            targetPlant = target.GetComponent<PlantController>();

            targetPlant.Size.On(OnSizeChanged);
        }

        protected virtual void OnSizeChanged(float currentSize)
        {
            transform.localScale = targetPlant.InitialScale * currentSize;
        }
    }
}
