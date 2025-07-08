using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumVisibilityController : MonoBehaviour
    {

        protected void Start()
        {
            TerrariumController.Instance.OnPlayerInside.AddListener(OnPlayerInside);
            gameObject.SetActive(false);
        }

        void OnPlayerInside(bool inside)
        {
            gameObject.SetActive(inside);
        }
    }
}
