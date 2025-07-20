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

        public Quaternion GetStemMiddleRotation() => stemMiddle.localRotation;

        protected override void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            base.OnEnvironmentChanged(_, __);

            var sunPos = TerrariumController.Instance.GetSunWorldPosition();
            var lookDir = (sunPos - transform.position).normalized;
            var upDir = (transform.position - transform.root.position).normalized;

            if (Vector3.Dot(lookDir, upDir) >= 0f)
            {
                stemMiddle.up = lookDir;
            }
            else
            {
                stemMiddle.up = upDir;
            }

        }
    }
}
