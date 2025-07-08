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
            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            gameObject.SetActive(false);
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            gameObject.SetActive(TerrariumController.Instance.IsPlayerInside() && TerrariumController.Instance.GetState() == StateData);
        }

        private void OnPlayerInside(bool inside)
        {
            gameObject.SetActive(TerrariumController.Instance.IsPlayerInside() && TerrariumController.Instance.GetState() == StateData);
        }
    }
}
