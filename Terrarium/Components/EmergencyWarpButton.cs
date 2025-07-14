using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class EmergencyWarpButton : MonoBehaviour
    {
        public Transform blackHolePivot;

        InteractReceiver interactReceiver;

        protected void Awake()
        {
            interactReceiver = GetComponent<InteractReceiver>();
            interactReceiver.OnPressInteract += OnPressInteract;
        }

        protected void OnDestroy()
        {
            interactReceiver.OnPressInteract -= OnPressInteract;
        }

        protected void Start()
        {
            var text = Terrarium.NewHorizons.GetTranslationForUI($"WW_TERRARIUM_EmergencyWarp");
            interactReceiver.ChangePrompt(text);
        }

        private void OnPressInteract()
        {
            TerrariumController.Instance.OnEmergencyWarp.Invoke(blackHolePivot);
        }
    }
}
