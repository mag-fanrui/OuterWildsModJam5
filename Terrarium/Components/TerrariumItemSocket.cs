using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumItemSocket : OWItemSocket
    {
        public override void Awake()
        {
            base.Awake();
            _acceptableType = TerrariumItem.ITEM_TYPE;
        }

        public override bool PlaceIntoSocket(OWItem item)
        {
            if (base.PlaceIntoSocket(item))
            {
                var terrariumItem = item as TerrariumItem;
                TerrariumController.Instance.SetState(terrariumItem.StateData);
                Locator.GetShipLogManager().RevealFact("WW_TERRARIUM_ITEM_INSERTED");
                return true;
            }
            return false;
        }

        public override OWItem RemoveFromSocket()
        {
            var item = base.RemoveFromSocket();
            if (item is TerrariumItem)
            {
                TerrariumController.Instance.SetState(null);
            }
            return item;
        }
    }
}
