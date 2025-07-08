using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class AtmosphereLayerController : MonoBehaviour
    {

        protected void Start()
        {
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            TerrariumController.Instance.Atmosphere.On(OnAtmosphereChanged);
            gameObject.SetActive(false);
        }

        void OnPlayerInside(bool inside)
        {
            UpdateVisibility();
        }

        void OnAtmosphereChanged(float value)
        {
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            var atmosphere = TerrariumController.Instance.Atmosphere.Current;
            var isVisible = TerrariumController.Instance.IsPlayerInside() && atmosphere > 0f;
            gameObject.SetActive(isVisible);
        }
    }
}
