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
        MeshRenderer[] renderers;

        protected virtual void Awake()
        {
            initialScale = transform.localScale;
            renderers = GetComponentsInChildren<MeshRenderer>();
            Size = ChaseValue.Create(this, 0.5f, OnSizeChanged, 0.02f);
        }

        protected virtual void Start()
        {
            TerrariumController.Instance.SunDistance.On(OnEnvironmentChanged);
            TerrariumController.Instance.SunAngle.On(OnEnvironmentChanged);
            TerrariumController.Instance.Humidity.On(OnEnvironmentChanged);
            TerrariumController.Instance.Atmosphere.On(OnEnvironmentChanged);
        }

        protected void OnGUI()
        {
            if (!Terrarium.DebugMode) return;
            var worldPos = transform.position;
            var camera = Locator.GetActiveCamera().transform;
            if (Vector3.Distance(camera.position, worldPos) > 100f) return;
            if (Vector3.Dot(worldPos - camera.position, camera.forward) < 0f) return;
            var screenPos = Locator.GetActiveCamera().WorldToScreenPoint(worldPos);
            var guiPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
            GUILayout.BeginArea(new Rect(guiPos.x, guiPos.y, 300f, 300f));
            GUILayout.Label($"Size: {Size.Current:F2}");
            GUILayout.Label($"Health: {CalculateHealth():F2}");
            GUILayout.Label($"Sunlight: {CalculateSunlightFactor():F2}");
            GUILayout.Label($"Humidity: {CalculateHumidityFactor():F2}");
            GUILayout.Label($"Atmosphere: {CalculateAtmosphereFactor():F2}");
            ExtraDebugGUI();
            GUILayout.EndArea();
        }

        protected virtual void ExtraDebugGUI() { }

        protected float CalculateSize() => 0.1f + CalculateHealth() * 0.9f;

        protected float CalculateHealth()
        {
            var health = CalculateSunlightFactor() * CalculateHumidityFactor() * CalculateAtmosphereFactor();
            return health;
        }

        protected float CalculateSunlightFactor()
        {
            var sunPos = TerrariumController.Instance.GetSunWorldPosition() - transform.root.position;
            var selfPos = transform.position - transform.root.position;
            var angle = Vector3.Angle(sunPos.normalized, selfPos.normalized);
            var sunBrightness = 1f - TerrariumController.Instance.SunDistance.Current;
            var sunDirectness = Mathf.Clamp01(1f - angle / 90f);

            var sunlight = sunBrightness * sunDirectness;

            var sunlightFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.SunlightPreference - sunlight) / PlantData.SunlightTolerance);

            return sunlightFactor;
        }

        protected float CalculateHumidityFactor()
        {
            var humidity = TerrariumController.Instance.Humidity.Current;
            var humidityFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.HumidityPreference - humidity) / PlantData.HumidityTolerance);
            return humidityFactor;
        }

        protected float CalculateAtmosphereFactor()
        {
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;
            var atmosphereFactor = Mathf.Clamp01(1f - Mathf.Abs(PlantData.AtmospherePreference - atmosphere) / PlantData.AtmosphereTolerance);
            return atmosphereFactor;
        }

        protected void OnSizeChanged(float currentSize)
        {
            transform.localScale = initialScale * currentSize;
        }

        protected virtual void OnEnvironmentChanged(float _)
        {
            var targetSize = CalculateSize();
            Size.Target = targetSize;
            foreach (var renderer in renderers)
            {
                renderer.material.color = Color.Lerp(PlantData.SickColor, PlantData.HealthyColor, CalculateHealth());
            }
        }
    }
}
