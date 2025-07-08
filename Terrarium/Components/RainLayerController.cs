using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class RainLayerController : MonoBehaviour
    {

        protected void Start()
        {
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            TerrariumController.Instance.Humidity.On(OnHumidityChanged);
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

        void UpdateVisibility()
        {
            var humidity = TerrariumController.Instance.Humidity.Current;
            var isVisible = TerrariumController.Instance.IsPlayerInside() && humidity >= 0.75f;
            gameObject.SetActive(isVisible);
        }
    }
}
