using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class SproutingFlowerController : PlantController
    {
        Animator animator;
        
        bool ShouldBloom() => CalculateSunlightFactor() >= PlantData.BloomThreshold;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
        }

        protected override void OnEnvironmentChanged(TerrariumParamType _, float __)
        {
            base.OnEnvironmentChanged(_, __);
            animator.SetBool("Open", ShouldBloom());
        }

        protected override void ExtraDebugGUI()
        {
            GUILayout.Label($"Blooming: {CalculateSunlightFactor():F2} > {PlantData.BloomThreshold:F2} ({ShouldBloom()})");
        }
    }
}
