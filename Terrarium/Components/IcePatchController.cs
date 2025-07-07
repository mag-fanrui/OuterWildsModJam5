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
            Size = ChaseValue.Create(this, 0.5f, OnSizeChanged);
            initialScale = transform.localScale;
        }

        protected void Start()
        {
            TerrariumController.Instance.SunDistance.On(OnSunChanged);
            TerrariumController.Instance.SunAngle.On(OnSunChanged);
        }

        void OnSizeChanged(float currentSize)
        {
            transform.localScale = initialScale * currentSize;
        }

        void OnSunChanged(float _)
        {
            var sunPower = 1f - TerrariumController.Instance.SunDistance.Current;
            var sunDir = (TerrariumController.Instance.GetSunWorldPosition() - transform.position).normalized;
            var selfDir = (transform.position - transform.root.position).normalized;
            var angle = Vector3.Angle(sunDir, selfDir);
            var heat = Mathf.Clamp01(sunPower - angle / 90f);
            Size.Target = 1f - heat;
        }
    }
}
