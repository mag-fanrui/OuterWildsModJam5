using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class LilypadController : PlantController
    {
        Vector3 initialPosition;

        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.root.InverseTransformPoint(transform.position);
        }

        protected override void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            base.OnEnvironmentChanged(_, __);

            var state = TerrariumController.Instance.GetState();
            if (!state)
            {
                transform.position = transform.root.TransformPoint(initialPosition);
                return;
            }

            var minRadius = state.WaterMinRadius;
            var maxRadius = state.WaterMaxRadius;
            var waterRadius = Mathf.Lerp(minRadius, maxRadius, TerrariumController.Instance.Humidity);

            var dynamicPosition = initialPosition.normalized * waterRadius;

            transform.position = transform.root.TransformPoint(dynamicPosition);
        }
    }
}
