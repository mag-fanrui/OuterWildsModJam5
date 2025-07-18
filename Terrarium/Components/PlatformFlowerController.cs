using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class PlatformFlowerController : MonoBehaviour
    {
        protected void Start()
        {
            if (Locator.GetShipLogManager().IsFactRevealed("WW_TERRARIUM_GOOD_ENDING"))
            {
                GetComponentInChildren<Animator>().SetBool("Open", true);
            }
        }
    }
}
