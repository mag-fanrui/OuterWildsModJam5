using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class CloudLayerController : MonoBehaviour
    {

        protected void Start()
        {
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            TerrariumController.Instance.Humidity.On(OnHumidityChanged);
            TerrariumController.Instance.Atmosphere.On(OnAtmosphereChanged);
            gameObject.SetActive(false);
        }

        void OnPlayerInside(bool inside)
        {
            UpdateVisibility();
        }

        void OnHumidityChanged(float value)
        {
            UpdateVisibility();
        }

        void OnAtmosphereChanged(float value)
        {
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            var humidity = TerrariumController.Instance.Humidity.Current;
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;
            var isVisible = TerrariumController.Instance.IsPlayerInside() &&
                            humidity >= 0.75f &&
                            atmosphere >= 0.75f;
            gameObject.SetActive(isVisible);
        }
    }
}
