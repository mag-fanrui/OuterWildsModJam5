using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumInterfaceController : MonoBehaviour
    {
        [SerializeField]
        OWItemSocket terrariumSocket;

        protected void Awake()
        {
            terrariumSocket._acceptableType = TerrariumItem.ITEM_TYPE;

            terrariumSocket.OnSocketablePlaced += OnSocketablePlaced;
            terrariumSocket.OnSocketableRemoved += OnSocketableRemoved;
        }

        protected void OnDestroy()
        {
            terrariumSocket.OnSocketableDonePlacing -= OnSocketablePlaced;
            terrariumSocket.OnSocketableRemoved -= OnSocketableRemoved;
        }

        void OnSocketablePlaced(OWItem item)
        {
            var terrariumItem = item as TerrariumItem;
            TerrariumController.Instance.SetState(terrariumItem.StateData);
        }

        void OnSocketableRemoved(OWItem item)
        {
            if (item is TerrariumItem)
            {
                TerrariumController.Instance.SetState(null);
            }
        }
    }
}
