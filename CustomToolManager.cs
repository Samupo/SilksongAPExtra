using HarmonyLib;
using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra
{
    public static class CustomToolManager
    {
        private static HashSet<CustomToolItem> registeredTools = new HashSet<CustomToolItem>();

        private static ToolItemList GetToolList()
        {
            ToolItemManager manager = ManagerSingleton<ToolItemManager>.Instance;

            return AccessTools
                .Field(typeof(ToolItemManager), "toolItems")
                .GetValue(manager) as ToolItemList;
        }

        public static void RegisterTool(CustomToolItem tool)
        {
            GetToolList().Add(tool);
            registeredTools.Add(tool);

            SaveState.Instance.items.items = SaveState.Instance.items.items.AddToArray(new Item(tool.name, tool.Type == ToolItemType.Skill ? ItemType.Spell : ItemType.Tool, null));
        }

        public static IEnumerable<CustomToolItem> GetEquippedTools()
        {
            foreach (CustomToolItem tool in registeredTools)
            {
                if (tool.IsEquipped) yield return tool;
            }
        }

        #region Registration Patches
        [HarmonyPatch(typeof(ItemSet), MethodType.Constructor)]
        internal static class ItemSetConstructorPatch
        {
            [HarmonyPostfix]
            private static void Postfix(ItemSet __instance)
            {
                foreach (CustomToolItem tool in CustomToolManager.registeredTools)
                {
                    __instance.items = __instance.items.AddToArray(new Item(tool.name, tool.Type == ToolItemType.Skill ? ItemType.Spell : ItemType.Tool, null));
                }
            }
        }


        [HarmonyPatch(typeof(ToolItem), "get_IsUnlockedNotHidden", new Type[0])]
        internal static class ToolItem_IsUnlockedNotHidden_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(ToolItem __instance, ref bool __result)
            {
                if (__instance is CustomToolItem customTool)
                {
                    __result = SaveState.Instance.receivedItems.Contains(customTool.name);
                }
            }
        }
        #endregion
    }
}
