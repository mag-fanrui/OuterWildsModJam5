using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Data
{
    [CreateAssetMenu]
    public class TerrariumStateData : ScriptableObject
    {
        [SerializeField]
        protected float waterMinRadius;
        [SerializeField]
        protected float waterMaxRadius;

        public float WaterMinRadius => waterMinRadius;
        public float WaterMaxRadius => waterMaxRadius;
    }
}
