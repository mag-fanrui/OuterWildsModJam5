using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ParameterAdjustButton : MonoBehaviour
    {
        public TerrariumParamType Parameter;
        public float Amount;

        InteractReceiver interactReceiver;

        protected void Awake()
        {
            interactReceiver = GetComponent<InteractReceiver>();
            interactReceiver.OnPressInteract += OnPressInteract;
        }

        protected void OnDestroy()
        {
            interactReceiver.OnPressInteract -= OnPressInteract;
        }

        protected void Start()
        {
            var text = Terrarium.Instance.NewHorizons.GetTranslationForUI($"WW_TERRARIUM_{(Amount >= 0 ? "Increase" : "Decrease")}{Parameter}");
            interactReceiver.ChangePrompt(text);
        }

        private void OnPressInteract()
        {
            var tc = TerrariumController.Instance;
            switch (Parameter)
            {
                case TerrariumParamType.SunDistance:
                    AdjustParameter(tc.SunDistance, tc.SunDistanceEnabled);
                    break;
                case TerrariumParamType.SunAngle:
                    AdjustParameter(tc.SunAngle, tc.SunAngleEnabled);
                    break;
                case TerrariumParamType.Humidity:
                    AdjustParameter(tc.Humidity, tc.HumidityEnabled);
                    break;
                case TerrariumParamType.Atmosphere:
                    AdjustParameter(tc.Atmosphere, tc.AtmosphereEnabled);
                    break;
                case TerrariumParamType.EnclosureAngle:
                    AdjustParameter(tc.EnclosureAngle, tc.EnclosureAngleEnabled);
                    break;
            }
        }

        private void AdjustParameter(ChaseValue value, ToggleValue enabledValue)
        {
            var newValue = Mathf.Clamp01(value.Target + Amount);
            if (newValue != value.Target && enabledValue.Value)
            {
                value.Target = newValue;
                Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.GearRotate_Light);
            }
            else
            {
                Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.GearRotate_Fail);
            }
            interactReceiver.ResetInteraction();
            interactReceiver.UpdatePromptVisibility();
        }
    }
}
