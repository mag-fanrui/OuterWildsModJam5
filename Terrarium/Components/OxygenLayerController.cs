using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class OxygenLayerController : MonoBehaviour
    {
        VisorRainEffectVolume rainEffect;
        OWTriggerVolume triggerVolume;

        protected void Awake()
        {
            rainEffect = GetComponent<VisorRainEffectVolume>();
            triggerVolume = GetComponent<OWTriggerVolume>();
        }

        protected void Start()
        {
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            TerrariumController.Instance.Humidity.On(OnHumidityChanged);
            TerrariumController.Instance.Atmosphere.On(OnAtmosphereChanged);
            gameObject.SetActive(false);
        }

        void OnPlayerInside(bool inside)
        {
            UpdateActiveState();

            if (inside)
            {
                if (!triggerVolume._active) triggerVolume.SetTriggerActivation(true);
                if (!triggerVolume.IsTrackingObject(Locator.GetPlayerDetector()))
                {
                    triggerVolume.AddObjectToVolume(Locator.GetPlayerDetector());
                }
            }
            else
            {
                if (triggerVolume.IsTrackingObject(Locator.GetPlayerDetector()))
                {
                    triggerVolume.RemoveObjectFromVolume(Locator.GetPlayerDetector());
                }
                if (triggerVolume._active) triggerVolume.SetTriggerActivation(false);
            }
        }

        void OnHumidityChanged(float value)
        {
            UpdateActiveState();
        }

        void OnAtmosphereChanged(float value)
        {
            UpdateActiveState();
        }

        void UpdateActiveState()
        {
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;
            var isActive = TerrariumController.Instance.IsPlayerInside() &&
                            atmosphere >= 0.25f;
            gameObject.SetActive(isActive);

            var humidity = TerrariumController.Instance.Humidity.Current;
            rainEffect.SetVolumeActivation(humidity >= 0.75f);
        }
    }
}
