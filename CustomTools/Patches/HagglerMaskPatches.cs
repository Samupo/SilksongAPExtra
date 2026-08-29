using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Patches
{
    internal class HagglerMaskPatches
    {
        [HarmonyPatch(typeof(ShopItem), nameof(ShopItem.Cost), MethodType.Getter)]
        public static class ShopItem_Cost_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result)
            {
                if (CustomTools.hagglerMask.IsEquipped)
                {
                    __result = (int)Math.Ceiling(__result * 0.8);
                }
            }
        }
    }
}
