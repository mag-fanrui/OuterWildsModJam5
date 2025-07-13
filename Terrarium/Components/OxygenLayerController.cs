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
        OWAudioSource audioSource;

        protected void Awake()
        {
            rainEffect = GetComponent<VisorRainEffectVolume>();
            audioSource = GetComponent<OWAudioSource>();
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
            var humidity = TerrariumController.Instance.Humidity.Current;
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;

            var hasAir = atmosphere >= 0.25f;
            var hasRain = hasAir && humidity >= 0.75f;

            rainEffect._dropletRate = hasRain ? 10f : 0f;
            rainEffect._streakRate = hasRain ? 1f : 0f;

            var ambientAudio = hasRain ? AudioType.GD_RainAmbient_LP : AudioType.TH_CanyonAmbienceDay_LP;

            if (audioSource.audioLibraryClip != ambientAudio)
            {
                audioSource.AssignAudioLibraryClip(ambientAudio);
            }

            var isActive = TerrariumController.Instance.IsPlayerInside() && hasAir;
            gameObject.SetActive(isActive);
        }
    }
}
