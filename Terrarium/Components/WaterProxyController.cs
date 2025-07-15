using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class WaterProxyController : MonoBehaviour
    {
        [HideInInspector]
        public ChaseValue Size;

        protected TerrariumItem item;

        protected void Awake()
        {
            item = GetComponentInParent<TerrariumItem>();
            Size = ChaseValue.Create(this, 0f, OnSizeChanged);
        }

        protected void Start()
        {
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.Humidity.On(OnHumidityChanged);
        }

        void OnSizeChanged(float value)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(item.StateData.WaterMinRadius, item.StateData.WaterMaxRadius, value) * 2f;
        }

        void OnStateChanged(TerrariumStateData state)
        {
            if (state != item.StateData)
            {
                Size.Target = 0f;
            } else
            {
                Size.Target = TerrariumController.Instance.Humidity.Target;
            }
        }

        void OnHumidityChanged(float value)
        {
            if (TerrariumController.Instance.GetState() != item.StateData) return;
            Size.Target = TerrariumController.Instance.Humidity.Target;
        }
    }
}
