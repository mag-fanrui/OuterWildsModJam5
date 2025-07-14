using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumStateController : MonoBehaviour
    {
        public TerrariumStateData StateData;

        protected void Start()
        {
            TerrariumController.Instance.EnclosureAngle.On(OnEnclosureAngleChanged);
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            gameObject.SetActive(false);
        }

        void OnEnclosureAngleChanged(float value)
        {
            transform.localEulerAngles = Vector3.up * (180f + value * 360f);
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            UpdateState();
        }

        private void OnPlayerInside(bool inside)
        {
            UpdateState();
        }

        void UpdateState()
        {
            gameObject.SetActive(TerrariumController.Instance.IsPlayerInside() && TerrariumController.Instance.GetState() == StateData);
        }
    }
}
