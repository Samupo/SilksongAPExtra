using HarmonyLib;
using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Patches
{
    internal class SilkseedPatches
    {
        [HarmonyPatch(typeof(PlayerData), "get_CurrentSilkRegenMax")]
        internal static class PlayerData_CurrentSilkRegenMax_MyPatch
        {
            private static void Postfix(ref int __result)
            {
                if (CustomTools.silkseed.IsEquipped)
                {
                    __result += 2;
                }
            }
        }
    }
}
