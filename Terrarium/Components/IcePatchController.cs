using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class IcePatchController : MonoBehaviour
    {
        [HideInInspector]
        public ChaseValue Size;

        Vector3 initialScale;

        protected void Awake()
        {
            initialScale = transform.localScale;
            Size = ChaseValue.Create(this, 0.5f, OnSizeChanged);
        }

        protected void Start()
        {
            TerrariumController.Instance.SunDistance.On(OnSunChanged);
            TerrariumController.Instance.SunAngle.On(OnSunChanged);
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
            GUILayout.Label($"Sunlight: {CalculateSunlightFactor():F2}");
            GUILayout.EndArea();
        }

        protected float CalculateSunlightFactor()
        {
            var sunPos = TerrariumController.Instance.GetSunWorldPosition() - transform.root.position;
            var selfPos = transform.position - transform.root.position;
            var angle = Vector3.Angle(sunPos.normalized, selfPos.normalized);
            var sunBrightness = 1f - TerrariumController.Instance.SunDistance.Current;
            var sunDirectness = Mathf.Clamp01(1f - angle / 90f);

            var sunlight = sunBrightness * sunDirectness;
            return sunlight;
        }

        void OnSizeChanged(float currentSize)
        {
            transform.localScale = initialScale * currentSize;
        }

        void OnSunChanged(float _)
        {
            var sunlight = CalculateSunlightFactor();
            Size.Target = 1f - sunlight;
        }
    }
}
