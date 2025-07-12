using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Components;

namespace Terrarium.Patches
{
    [HarmonyPatch(typeof(NomaiTranslatorProp))]
    public static class NomaiTranslatorPropPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(NomaiTranslatorProp.SetNomaiText), typeof(NomaiText), typeof(int))]
        public static void SetNomaiText(NomaiTranslatorProp __instance)
        {
            var text = __instance._textNodeToDisplay;
            if (text.Contains("$WW_TERRARIUM_"))
            {
                text = text.Replace("$WW_TERRARIUM_SYSTEM_COUNT$", TerrariumComputerController.Instance.GetSystemCountString());
                text = text.Replace("$WW_TERRARIUM_CANDIDATE_COUNT$", TerrariumComputerController.Instance.GetCandidateCountString());
                __instance._textNodeToDisplay = text;
            }
        }
    }
}
