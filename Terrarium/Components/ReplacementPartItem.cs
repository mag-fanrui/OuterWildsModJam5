using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;
using UnityEngine;

namespace Terrarium.Components
{
    public class ReplacementPartItem : OWItem
    {
        public static Dictionary<TerrariumParamType, ItemType> PARAM_ITEM_TYPES = new()
        {
            { TerrariumParamType.SunDistance, EnumUtils.Create<ItemType>("ReplacementPart_SunDistance") },
            { TerrariumParamType.SunAngle, EnumUtils.Create<ItemType>("ReplacementPart_SunAngle") },
            { TerrariumParamType.Humidity, EnumUtils.Create<ItemType>("ReplacementPart_Humidity") },
            { TerrariumParamType.Atmosphere, EnumUtils.Create<ItemType>("ReplacementPart_Atmosphere") },
            { TerrariumParamType.EnclosureAngle, EnumUtils.Create<ItemType>("ReplacementPart_EnclosureAngle") },
        };

        public TerrariumParamType Parameter;

        string displayName;

        public override void Awake()
        {
            base.Awake();
            _type = PARAM_ITEM_TYPES[Parameter];
            displayName = Terrarium.NewHorizons.GetTranslationForUI($"WW_TERRARIUM_ITEM_ReplacementPart_{Parameter}");
        }

        public override string GetDisplayName() => displayName;

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.ToolItemWarpCorePickUp);
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
