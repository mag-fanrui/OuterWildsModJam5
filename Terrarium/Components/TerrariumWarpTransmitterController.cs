using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumWarpTransmitterController : MonoBehaviour
    {
        static TerrariumWarpTransmitterController instance;

        public static TerrariumWarpTransmitterController Instance => instance;

        NomaiWarpTransmitter warpTransmitter;

        public NomaiWarpTransmitter GetWarpTransmitter() => warpTransmitter;

        protected void Awake()
        {
            instance = this;
            warpTransmitter = GetComponent<NomaiWarpTransmitter>();
        }

    }
}
