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
    [HarmonyPatch(typeof(ProbeLauncher))]
    public static class ProbeLauncherPatches
    {
        [HarmonyPrefix, HarmonyPatch(nameof(ProbeLauncher.LaunchProbe))]
        public static bool LaunchProbe(ProbeLauncher __instance)
        {
            if (TerrariumController.Instance && TerrariumController.Instance.IsPlayerInside())
            {
                // Same as vanilla just without the scout boost velocity. Ouch
                if (__instance.IsLaunchObstructed())
                {
                    NotificationData notificationData = new NotificationData(__instance._notificationFilter, UITextLibrary.GetString(UITextType.NotificationProbeLaunchObstructed), 2f, false);
                    NotificationManager.SharedInstance.PostNotification(notificationData, false);
                    Locator.GetPlayerAudioController().PlayNegativeUISound();
                    return false;
                }
                __instance._activeProbe = Locator.GetProbe();
                __instance._allowRetrieval = false;
                __instance._preLaunchProbeProxy.SetActive(false);
                float num = __instance.GetMaxLaunchSpeed();
                bool flag = false;
                if (__instance._promptTrigger != null)
                {
                    __instance._promptTrigger.OnLaunchProbe();
                }
                if (__instance._promptTrigger != null && __instance._promptTrigger.GetOverrideLaunchSpeed())
                {
                    num = __instance._promptTrigger.GetLaunchSpeed();
                }
                else if (Locator.GetPlayerRulesetDetector().GetProbeRuleSet() != null && Locator.GetPlayerRulesetDetector().GetProbeRuleSet().GetUseProbeSpeedOverride())
                {
                    num = Locator.GetPlayerRulesetDetector().GetProbeRuleSet().GetProbeSpeedOverride();
                }
                else if (Locator.GetPlayerRulesetDetector().GetPlanetoidRuleset() != null && Locator.GetPlayerRulesetDetector().GetPlanetoidRuleset().GetHorizonRadius() > 0f)
                {
                    bool flag2 = false;
                    RaycastHit raycastHit;
                    if (Physics.Raycast(__instance._launcherTransform.position, __instance._launcherTransform.forward, out raycastHit, 1000f, OWLayerMask.physicalMask) && raycastHit.rigidbody.mass > 100f && raycastHit.rigidbody != Locator.GetPlayerRulesetDetector().GetPlanetoidRuleset().GetAttachedOWRigidbody()
                        .GetRigidbody())
                    {
                        flag2 = true;
                    }
                    if (!flag2 && __instance._orbitSpeed > 0f)
                    {
                        if (__instance._orbitLaunchAngle > -10f && __instance._orbitLaunchAngle < 50f)
                        {
                            num = ((__instance._orbitLaunchAngle > 0f) ? (__instance._orbitSpeed * 1.1f / Mathf.Cos(__instance._orbitLaunchAngle * 0.017453292f)) : __instance._orbitSpeed);
                            flag = true;
                        }
                        else if (__instance._orbitLaunchAngle <= -10f)
                        {
                            num = __instance._orbitSpeed;
                        }
                    }
                }
                OWRigidbody attachedOWRigidbody = Locator.GetPlayerTransform().GetAttachedOWRigidbody(false);
                Vector3 vector = attachedOWRigidbody.GetVelocity() + __instance._launcherTransform.forward * num;
                //Vector3 vector2 = (attachedOWRigidbody.GetVelocity() - vector) * 0.05f;
                //attachedOWRigidbody.AddVelocityChange(vector2);
                __instance._activeProbe.Launch(__instance._launcherTransform, vector, flag, __instance.GetHorizonRadiusOverride());
                bool flag3 = __instance._fluidDetector != null && __instance._fluidDetector.InFluidType(FluidVolume.Type.WATER);
                __instance._effects.PlayLaunchClip(flag3);
                __instance._effects.PlayLaunchParticles(flag3);
                if (__instance._name == ProbeLauncher.Name.Player && num > 20f)
                {
                    RumbleManager.PulseProbeLaunch();
                }
                GlobalMessenger<SurveyorProbe>.FireEvent("LaunchProbe", __instance._activeProbe);
                //__instance.OnLaunchProbe?.Invoke(__instance._activeProbe);
                return false;
            }
            return true;
        }
    }
}
