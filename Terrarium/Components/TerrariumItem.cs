using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Data;
using UnityEngine;

namespace Terrarium.Components
{
    public class TerrariumItem : OWItem
    {
        public static ItemType ITEM_TYPE = EnumUtils.Create<ItemType>(nameof(TerrariumItem));

        public TerrariumStateData StateData;

        [SerializeField]
        protected Vector3 holdOffset;
        [SerializeField]
        protected string pickupFact;

        string displayName;

        public override void Awake()
        {
            base.Awake();
            _type = ITEM_TYPE;
            displayName = StateData ? Terrarium.NewHorizons.GetTranslationForUI($"WW_TERRARIUM_ITEM_{StateData.name}") : "ERROR";
        }

        public override string GetDisplayName() => displayName;

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.ToolItemWarpCorePickUp);
            transform.localPosition = holdOffset;
            Locator.GetShipLogManager().RevealFact(pickupFact);
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.ToolItemWarpCoreDrop);
        }

        public override void SocketItem(Transform socketTransform, Sector sector)
        {
            base.SocketItem(socketTransform, sector);
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.ToolItemWarpCoreInsert);
        }

        public override void OnCompleteUnsocket()
        {
            base.OnCompleteUnsocket();
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.ToolItemWarpCoreRemove);
        }
    }
}
