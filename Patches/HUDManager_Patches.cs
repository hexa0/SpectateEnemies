﻿using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpectateEnemy.Patches
{
    [HarmonyPatch(typeof(HUDManager), "Update")]
    internal class HUDManager_Patches
    {
        private static void Postfix(HUDManager __instance)
        {
            if (GameNetworkManager.Instance.localPlayerController.isPlayerDead)
            {
                if (StartOfRound.Instance.shipIsLeaving)
                {
                    Light light = __instance.playersManager.spectateCamera.GetComponent<Light>();
                    if (light != null)
                    {
                        light.enabled = false;
                    }
                    return;
                }
                string key = InputControlPath.ToHumanReadableString(__instance.playerActions.Movement.Interact.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                // who needs to change the y position when u can just \n :sunglasses:
                __instance.holdButtonToEndGameEarlyText.text += $"\n\n\n\n\nSwitch to {(SpectateEnemies.Instance.SpectatingEnemies ? "Players" : "Enemies")}: [{key}]\nToggle Flashlight : [RMB] (Click)\nEnemy Menu : [Insert]";
                if (__instance.playerActions.Movement.PingScan.WasReleasedThisFrame())
                {
                    // flashlight already exists on spectator camera, thanks zeekerss
                    Light light = __instance.playersManager.spectateCamera.GetComponent<Light>();
                    if (light != null)
                    {
                        light.enabled = !light.enabled;
                    }
                }
            }
        }
    }
}
