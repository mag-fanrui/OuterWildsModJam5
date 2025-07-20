using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxyLilypadController : ProxyPlantController
    {
        protected void Start()
        {
            TerrariumController.Instance.OnParameterChanged.AddListener(OnEnvironmentChanged);
        }

        void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            transform.localPosition = (targetPlant as LilypadController).transform.localPosition;
        }
    }
}
