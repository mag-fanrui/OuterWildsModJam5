using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class ArtificialSunController : MonoBehaviour, ILightSource
    {
        public static LightSourceType LIGHT_SOURCE_TYPE = EnumUtils.Create<LightSourceType>(nameof(ArtificialSunController));

        public bool CheckIlluminationAtPoint(Vector3 point, float buffer, float maxDistance) => true;

        public OWLight2[] GetLights() => null;

        public LightSourceType GetLightSourceType() => LIGHT_SOURCE_TYPE;

        public Vector3 GetLightSourceOcclusionPoint(Vector3 sensorWorldPos)
        {
            Vector3 sunPos = TerrariumController.Instance.GetSunWorldPosition();
            Vector3 terrariumPos = TerrariumController.Instance.GetTerrariumWorldPosition();
            float terrariumRadius = TerrariumController.Instance.GetTerrariumRadius();

            var dir = (sunPos - sensorWorldPos).normalized;
            var exitPoint = FindRaySphereExitPoint(sensorWorldPos, dir, terrariumPos, terrariumRadius);
            return exitPoint;
        }

        protected void LateUpdate()
        {
            transform.position = TerrariumController.Instance.GetSunWorldPosition();
        }

        Vector3 FindRaySphereExitPoint(Vector3 start, Vector3 dir, Vector3 center, float radius)
        {
            Vector3 m = start - center;
            Vector3 d = dir.normalized;

            float b = 2f * Vector3.Dot(m, d);
            float c = Vector3.Dot(m, m) - radius * radius;

            float discriminant = b * b - 4f * c;

            if (discriminant < 0)
            {
                // No real intersection
                return start;
            }

            // Solve quadratic
            float sqrtDiscriminant = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtDiscriminant) / 2f;
            float t2 = (-b + sqrtDiscriminant) / 2f;

            // Since the ray starts inside the sphere, we want the **positive t**
            float t = Mathf.Max(t1, t2);

            return start + d * t;
        }
    }
}
