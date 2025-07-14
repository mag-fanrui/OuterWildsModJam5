using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Data
{
    [CreateAssetMenu]
    public class TerrariumPlantData : ScriptableObject
    {
        [SerializeField]
        float minScale = 0.1f;
        [SerializeField]
        float sunlightPreference = 0.5f;
        [SerializeField]
        float sunlightTolerance = 0.5f;
        [SerializeField]
        float humidityPreference = 0.5f;
        [SerializeField]
        float humidityTolerance = 0.5f;
        [SerializeField]
        float atmospherePreference = 0.5f;
        [SerializeField]
        float atmosphereTolerance = 0.5f;
        [SerializeField]
        bool useHealthColors = false;
        [SerializeField]
        Color healthyColor = Color.white;
        [SerializeField]
        Color sickColor = Color.gray;
        [SerializeField]
        float bloomThreshold = -1f;

        public float MinScale => minScale;
        public float SunlightPreference => sunlightPreference;
        public float SunlightTolerance => sunlightTolerance;
        public float HumidityPreference => humidityPreference;
        public float HumidityTolerance => humidityTolerance;
        public float AtmospherePreference => atmospherePreference;
        public float AtmosphereTolerance => atmosphereTolerance;
        public bool UseHealthColors => useHealthColors;
        public Color HealthyColor => healthyColor;
        public Color SickColor => sickColor;
        public float BloomThreshold => bloomThreshold;
    }
}
