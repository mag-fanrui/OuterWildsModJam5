using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class AtmosphereProxyController : MonoBehaviour
    {
        [HideInInspector]
        public ChaseValue Opacity;
        [SerializeField]
        protected float minOpacity;
        [SerializeField]
        protected float maxOpacity;

        protected TerrariumItem item;
        protected MeshRenderer renderer;
        protected Material material;

        protected void Awake()
        {
            item = GetComponentInParent<TerrariumItem>();
            renderer = GetComponent<MeshRenderer>();
            material = new Material(renderer.sharedMaterial);
            renderer.sharedMaterial = material;
            Opacity = ChaseValue.Create(this, 0f, OnOpacityChanged);
        }

        protected void Start()
        {
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.Atmosphere.On(OnAtmosphereChanged);
        }

        void OnOpacityChanged(float value)
        {
            var c = material.color;
            c.a = Mathf.Lerp(minOpacity, maxOpacity, value);
            material.color = c;
        }

        void OnStateChanged(TerrariumStateData state)
        {
            if (state != item.StateData)
            {
                Opacity.Target = 0f;
            }
            else
            {
                Opacity.Target = TerrariumController.Instance.Atmosphere.Target;
            }
        }

        void OnAtmosphereChanged(float value)
        {
            if (TerrariumController.Instance.GetState() != item.StateData) return;
            Opacity.Target = TerrariumController.Instance.Atmosphere.Target;
        }
    }
}
