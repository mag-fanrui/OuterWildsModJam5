using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class WaterLayerController : MonoBehaviour
    {
        [HideInInspector]
        public ChaseValue WaterLevel;

        float minRadius;
        float maxRadius;
        Material oceanFogMaterial;
        RadialFluidVolume fluidVolume;

        public float MinRadius
        {
            get => minRadius;
            set
            {
                minRadius = value;
                OnWaterLevelChanged(WaterLevel);
            }
        }

        public float MaxRadius
        {
            get => maxRadius;
            set
            {
                maxRadius = value;
                OnWaterLevelChanged(WaterLevel);
            }
        }

        protected void Awake()
        {
            oceanFogMaterial = transform.Find("OceanFog").GetComponent<MeshRenderer>().material;
            oceanFogMaterial.SetFloat("_Radius2", 0);
            fluidVolume = transform.Find("WaterVolume").GetComponent<RadialFluidVolume>();
            maxRadius = transform.localScale.x;
            WaterLevel = ChaseValue.Create(this, 0.5f, OnWaterLevelChanged);
        }

        protected void Start()
        {
            TerrariumController.Instance.Humidity.On(OnHumidityChanged);
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            gameObject.SetActive(false);
        }

        void OnWaterLevelChanged(float value)
        {
            var actualRadius = maxRadius * value;
            transform.localScale = Vector3.one * actualRadius;
            oceanFogMaterial.SetFloat("_Radius", actualRadius);
            fluidVolume._radius = actualRadius;
        }

        void OnHumidityChanged(float value)
        {
            WaterLevel.Target = value;
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            if (stateData != null)
            {
                minRadius = stateData.WaterMinRadius;
                maxRadius = stateData.WaterMaxRadius;
                OnWaterLevelChanged(WaterLevel);
            }
        }

        void OnPlayerInside(bool inside)
        {
            gameObject.SetActive(inside);
        }
    }
}
