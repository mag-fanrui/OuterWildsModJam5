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
        float initialRadius;

        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.parent.InverseTransformPoint(transform.position);
            initialRadius = Vector3.Distance(transform.position, transform.parent.position);
        }

        protected override void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            base.OnEnvironmentChanged(_, __);

            var state = TerrariumController.Instance.GetState();
            if (!state)
            {
                transform.position = transform.parent.TransformPoint(initialPosition);
                return;
            }

            var minRadius = state.WaterMinRadius;
            var maxRadius = state.WaterMaxRadius;
            var waterRadius = Mathf.Lerp(minRadius, maxRadius, TerrariumController.Instance.Humidity);

            var placementRadius = Mathf.Max(initialRadius, waterRadius);

            var dynamicPosition = initialPosition.normalized * placementRadius;

            transform.position = transform.parent.TransformPoint(dynamicPosition);
        }
    }
}
