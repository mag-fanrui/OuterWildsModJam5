using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxySunflowerController : ProxyPlantController
    {
        [SerializeField]
        protected Transform stemMiddle;

        protected void Start()
        {
            TerrariumController.Instance.OnParameterChanged.AddListener(OnEnvironmentChanged);
        }

        void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            var stemRot = (targetPlant as SunflowerController).GetStemMiddleRotation();
            stemMiddle.localRotation = stemRot;
        }
    }
}
