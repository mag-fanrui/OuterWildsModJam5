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
        NomaiWarpReceiver warpReceiver;
        Vector3 initialPosition;
        Quaternion initialRotation;
        bool emergencyWarp;

        protected void Awake()
        {
            warpReceiver = gameObject.GetComponent<NomaiWarpReceiver>();
            warpReceiver._warpRadius = 1f;
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
            warpReceiver._blackHole.OnCollapse += OnCollapse;
            warpReceiver.OnReceivePlayerBody += OnReceivePlayerBody;
        }

        protected void OnDestroy()
        {
            warpReceiver._blackHole.OnCollapse -= OnCollapse;
            warpReceiver.OnReceivePlayerBody -= OnReceivePlayerBody;
        }

        protected void Start()
        {
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.OnEmergencyWarp.AddListener(OnEmergencyWarp);
            gameObject.SetActive(false);
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            gameObject.SetActive(stateData != null);
        }

        private void OnEmergencyWarp(Transform warpOrigin)
        {
            if (emergencyWarp) return;

            transform.position = warpOrigin.position;
            transform.rotation = warpOrigin.rotation;
            emergencyWarp = true;
        }

        void OnCollapse()
        {
            if (!emergencyWarp) return;
            
            emergencyWarp = false;
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
        }

        void OnReceivePlayerBody()
        {
            Locator.GetShipLogManager().RevealFact("WW_TERRARIUM_MINI_WARP");
        }
    }
}
