using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Components;
using UnityEngine;

namespace Terrarium.Patches
{
    [HarmonyPatch(typeof(SingleLightSensor))]
    public static class SingleLightSensorPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(SingleLightSensor.UpdateIllumination))]
        public static void UpdateIllumination(SingleLightSensor __instance)
        {
            for (int i = 0; i < __instance._lightSources.Count; i++)
            {
                if (__instance._lightSources[i] is ArtificialSunController artificialSun)
                {
                    var sensorPos = __instance.transform.TransformPoint(__instance._localSensorOffset);
                    var sensorDir = Vector3.zero;
                    if (__instance._directionalSensor)
                    {
                        sensorDir = __instance.transform.TransformDirection(__instance._localDirection).normalized;
                    }
                    if (artificialSun.CheckIlluminationAtPoint(sensorPos, __instance._sensorRadius, __instance._maxDistance) && !__instance.CheckOcclusion(artificialSun.GetLightSourceOcclusionPoint(sensorPos), sensorPos, sensorDir, true))
                    {
                        __instance._illuminated = true;
                    }
                }
            }
        }
    }
}
