using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class PlantController : MonoBehaviour
    {
        public TerrariumPlantData PlantData;
        [HideInInspector]
        public ChaseValue Size;

        Vector3 initialScale;

        protected virtual void Awake()
        {
            Size = ChaseValue.Create(this, 0.5f, OnSizeChanged);
            initialScale = transform.localScale;
        }

        protected virtual void Start()
        {
            TerrariumController.Instance.SunDistance.On(OnEnvironmentChanged);
            TerrariumController.Instance.SunAngle.On(OnEnvironmentChanged);
            TerrariumController.Instance.Humidity.On(OnEnvironmentChanged);
            TerrariumController.Instance.Atmosphere.On(OnEnvironmentChanged);
        }

        protected virtual float CalculateSize() => CalculateHealth();

        protected virtual float CalculateHealth()
        {
            var sunPos = transform.root.InverseTransformPoint(TerrariumController.Instance.GetSunWorldPosition());
            var selfPos = transform.root.InverseTransformPoint(transform.position);
            var angle = Vector3.Angle(sunPos.normalized, selfPos.normalized);
            var sunBrightness = 1f - TerrariumController.Instance.SunDistance.Current;
            var sunDirectness = Mathf.Clamp01(1f - angle / 90f);

            var sunlight = sunBrightness * sunDirectness;

            var humidity = TerrariumController.Instance.Humidity.Current;
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;

            var sunlightFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.SunlightPreference - sunlight) / PlantData.SunlightTolerance);

            var humidityFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.HumidityPreference - humidity) / PlantData.HumidityTolerance);

            var atmosphereFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.AtmospherePreference - atmosphere) / PlantData.AtmosphereTolerance);

            var health = sunlightFactor * humidityFactor * atmosphereFactor;
            return health;
        }

        protected virtual void OnSizeChanged(float currentSize)
        {
            transform.localScale = initialScale * currentSize;
        }

        protected virtual void OnEnvironmentChanged(float _)
        {
            var targetSize = CalculateSize();
            Size.Target = targetSize;
        }
    }
}
