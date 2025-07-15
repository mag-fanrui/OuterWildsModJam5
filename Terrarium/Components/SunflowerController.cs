using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class SunflowerController : PlantController
    {
        [SerializeField]
        protected Transform stemMiddle;
        [SerializeField]
        protected Transform stemEnd;

        protected override void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            base.OnEnvironmentChanged(_, __);

            var sunPos = TerrariumController.Instance.GetSunWorldPosition();
            var lookDir = (sunPos - transform.position).normalized;
            stemMiddle.up = lookDir;
        }
    }
}
