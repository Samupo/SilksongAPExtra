using HarmonyLib;
using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra
{
    public static class CustomToolManager
    {
        private static HashSet<CustomToolItem> registeredTools = new HashSet<CustomToolItem>();
        private static ItemSet patchedItemSet = null;
        private static ToolItemManager patchedToolItemManager = null;

        private static ToolItemList GetToolList(ToolItemManager manager)
        {
            return AccessTools
                .Field(typeof(ToolItemManager), "toolItems")
                .GetValue(manager) as ToolItemList;
        }

        public static void RegisterTool(CustomToolItem tool)
        {
            registeredTools.Add(tool);
            patchedItemSet = null;
            patchedToolItemManager = null;
        }

        public static IEnumerable<CustomToolItem> GetEquippedTools()
        {
            foreach (CustomToolItem tool in registeredTools)
            {
                if (tool.IsEquipped) yield return tool;
            }
        }


        #region Registration Patches
        public static void TryPatchTools(ToolItemManager manager)
        {
            if (manager == null) return;
            if (patchedToolItemManager != manager)
            {
                foreach (CustomToolItem tool in CustomToolManager.registeredTools)
                {
                    if (!GetToolList(manager).Contains(tool)) GetToolList(manager).Add(tool);
                }
                patchedToolItemManager = manager;
            }
        }

        static void TryPatchItems(SaveState saveData)
        {
            if (patchedItemSet != saveData.items && registeredTools.Count > 0)
            {
                SilksongAPExtraPlugin.Instance.Log.LogWarning("Items before patch: " + saveData.items.items.Length);
                foreach (CustomToolItem tool in CustomToolManager.registeredTools)
                {
                    bool alreadyExists = saveData.items.items.Any(item => item.Name == tool.name);
                    if (!alreadyExists) saveData.items.items = saveData.items.items.AddToArray(new Item(tool.name, tool.Type == ToolItemType.Skill ? ItemType.Spell : ItemType.Tool, null));
                }
                patchedItemSet = saveData.items;
                SilksongAPExtraPlugin.Instance.Log.LogWarning("Items after patch: " + saveData.items.items.Length);
            }
        }

        [HarmonyPatch(typeof(SaveState), nameof(SaveState.GetItem))]
        internal static class PatchNewItems
        {
            [HarmonyPrefix]
            private static void Prefix(SaveState __instance)
            {
                TryPatchItems(__instance);
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
