using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class FluidPumpController : MonoBehaviour
    {
        public TerrariumParamType Parameter;

        [SerializeField]
        Transform pumpArm;
        [SerializeField]
        Transform fluid;

        protected void Start()
        {
            TerrariumController.Instance.OnParameterChanged.AddListener(OnParameterChanged);
            UpdateVisuals();
        }

        void OnParameterChanged(TerrariumParamType param, float value)
        {
            UpdateVisuals();
        }

        void UpdateVisuals()
        {
            var value = TerrariumController.Instance.GetParameter(Parameter).Current;
            pumpArm.localPosition = Vector3.up * Mathf.Lerp(0.31125f, 0.91125f, value);
            fluid.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.2f, value), 1f);
        }
    }
}
