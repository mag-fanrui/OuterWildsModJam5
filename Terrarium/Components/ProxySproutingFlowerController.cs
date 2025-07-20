using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ProxySproutingFlowerController : ProxyPlantController
    {
        Animator animator;

        protected void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        protected void Start()
        {
            TerrariumController.Instance.OnParameterChanged.AddListener(OnEnvironmentChanged);
        }

        void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            animator.SetBool("Open", (targetPlant as SproutingFlowerController).ShouldBloom());
        }
    }
}
