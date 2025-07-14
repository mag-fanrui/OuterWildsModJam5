using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class SunflowerController : PlantController
    {
        [SerializeField]
        Transform stemMiddle;
        [SerializeField]
        Transform stemEnd;

        protected override void OnEnvironmentChanged(float _)
        {
            base.OnEnvironmentChanged(_);

            var upDir = (transform.position - transform.root.position).normalized;
            var sunPos = TerrariumController.Instance.GetSunWorldPosition();
            var sunDir = (sunPos - transform.root.position).normalized;

            var lookDir = (sunPos - transform.position).normalized;
            
            stemMiddle.up = lookDir;
        }
    }
}
