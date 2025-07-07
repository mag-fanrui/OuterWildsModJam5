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

        protected void Awake()
        {
            gameObject.SetActive(false);

            TerrariumController.Instance.OnStateChanged.AddListener(OnStateChanged);
        }

        void OnStateChanged(TerrariumStateData stateData)
        {
            gameObject.SetActive(stateData == StateData);
        }
    }
}
