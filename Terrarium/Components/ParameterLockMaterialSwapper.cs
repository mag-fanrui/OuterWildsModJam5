using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ParameterLockMaterialSwapper : MonoBehaviour
    {
        [SerializeField]
        protected TerrariumParamType parameter;
        [SerializeField]
        protected Material lockedMaterial;
        [SerializeField]
        protected Material unlockedMaterial;

        Renderer renderer;

        protected void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        protected void Start()
        {
            TerrariumController.Instance.GetParameterEnabled(parameter).On(OnParamEnableChanged);
        }

        void OnParamEnableChanged(bool enabled)
        {
            renderer.sharedMaterial = enabled ? unlockedMaterial : lockedMaterial;
        }
    }
}
