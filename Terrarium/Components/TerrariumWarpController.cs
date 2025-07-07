using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumWarpController : MonoBehaviour
    {
        NomaiWarpTransmitter warpTransmitter;

        protected void Awake()
        {
            warpTransmitter = GetComponent<NomaiWarpTransmitter>();
            warpTransmitter.OnReceiveWarpedBody += OnReceiveWarpedBody;

            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
        }

        protected void OnDestroy()
        {
            warpTransmitter.OnReceiveWarpedBody -= OnReceiveWarpedBody;
        }

        protected void Update()
        {
            var shouldBeOpen = TerrariumController.Instance.GetState() != null && !warpTransmitter.IsPlayerOnPlatform() && !warpTransmitter.IsProbeOnPlatform() && warpTransmitter._targetReceiver != null;
            if (warpTransmitter.IsBlackHoleOpen() != shouldBeOpen)
            {
                if (shouldBeOpen)
                {
                    warpTransmitter.OpenBlackHole(warpTransmitter._targetReceiver, true);
                    enabled = false;
                }
                else
                {
                    warpTransmitter.CloseBlackHole();
                    enabled = false;
                }
            }
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            enabled = true;
        }

        void OnReceiveWarpedBody(OWRigidbody warpedBody, NomaiWarpPlatform startPlatform, NomaiWarpPlatform targetPlatform)
        {
            enabled = true;
        }
    }
}
