using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace SilksongAPExtra
{
    [HarmonyPatch]
    internal static class ToolPatches
    {
        private const string InvalidSkillMessage = "Fsm Skill Tool Event {0} is invalid";
        private const string InvalidToolMessage = "Fsm Tool Event {0} is invalid";

        private static readonly AccessTools.FieldRef<HeroController, ToolItem> WillThrowToolRef =
            AccessTools.FieldRefAccess<HeroController, ToolItem>("willThrowTool");

        private static readonly MethodInfo LogErrorFormatMethod = AccessTools.Method(
            typeof(Debug),
            nameof(Debug.LogErrorFormat),
            new[]
            {
            typeof(UnityEngine.Object),
            typeof(string),
            typeof(object[])
            });

        private static readonly MethodInfo InvalidSkillReplacementMethod =
            AccessTools.Method(
                typeof(ToolPatches),
                nameof(InvalidSkillToolEventReplacement));

        private static readonly MethodInfo InvalidToolReplacementMethod =
            AccessTools.Method(
                typeof(ToolPatches),
                nameof(InvalidToolEventReplacement));

        [HarmonyPatch(typeof(HeroController), "ThrowTool", new[] { typeof(bool) })]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ThrowToolTranspiler(
            IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo pendingReplacement = null;

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr &&
                    instruction.operand is string text)
                {
                    if (text == InvalidSkillMessage)
                    {
                        pendingReplacement = InvalidSkillReplacementMethod;
                    }
                    else if (text == InvalidToolMessage)
                    {
                        pendingReplacement = InvalidToolReplacementMethod;
                    }
                }

                if (pendingReplacement != null &&
                    instruction.opcode == OpCodes.Call &&
                    Equals(instruction.operand, LogErrorFormatMethod))
                {
                    instruction.operand = pendingReplacement;
                    pendingReplacement = null;
                }

                yield return instruction;
            }
        }

        private static void InvalidSkillToolEventReplacement(
            UnityEngine.Object context,
            string format,
            object[] args)
        {
            HeroController hero = context as HeroController;

            ToolItem tool = hero != null
                ? WillThrowToolRef(hero)
                : null;

            string eventName = GetEventName(args);

            OnInvalidSkillToolEvent(
                hero,
                tool,
                eventName);
        }

        private static void InvalidToolEventReplacement(
            UnityEngine.Object context,
            string format,
            object[] args)
        {
            HeroController hero = context as HeroController;

            ToolItem tool = hero != null
                ? WillThrowToolRef(hero)
                : null;

            string eventName = GetEventName(args);

            OnInvalidToolEvent(
                hero,
                tool,
                eventName);
        }

        private static string GetEventName(object[] args)
        {
            if (args == null ||
                args.Length == 0 ||
                args[0] == null)
            {
                return null;
            }

            return args[0].ToString();
        }

        private static void OnInvalidSkillToolEvent(
            HeroController hero,
            ToolItem tool,
            string eventName)
        {
            if (tool is CustomToolItem customTool)
            {
                customTool.OnUsed();
            }
        }

        private static void OnInvalidToolEvent(
            HeroController hero,
            ToolItem tool,
            string eventName)
        {
            if (tool is CustomToolItem customTool)
            {
                customTool.OnUsed();
            }
        }

        private static readonly AccessTools.FieldRef<HeroController, ToolItem> WillThrowToolField =
            AccessTools.FieldRefAccess<HeroController, ToolItem>("willThrowTool");

        private static readonly AccessTools.FieldRef<HeroController, double> CanThrowTimeField =
            AccessTools.FieldRefAccess<HeroController, double>("canThrowTime");

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HeroController), "CanThrowTool",
            new Type[]
            {
                typeof(ToolItem),
                typeof(AttackToolBinding),
                typeof(bool)
            })]
        private static bool CanThrowToolPrefix(
            ToolItem tool,
            AttackToolBinding binding,
            bool reportFailure,
            ref bool __result)
        {
            if (!(tool is CustomToolItem customTool))
            {
                return true;
            }

            bool canUse = true;

            if (tool.Type == ToolItemType.Red)
            {
                canUse = !tool.IsEmpty;
            }

            if (!canUse)
            {
                if (reportFailure)
                {
                    ToolItemManager.ReportBoundAttackToolFailed(binding);
                    customTool.OnUsageFailed();
                }

                __result = false;
                return false;
            }

            __result = true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HeroController), "ThrowTool")]
        private static bool ThrowToolPrefix(
            HeroController __instance,
            bool isAutoThrow)
        {
            ToolItem tool = WillThrowToolField(__instance);

            if (!(tool is CustomToolItem customTool))
            {
                return true;
            }

            if (!isAutoThrow && Time.timeAsDouble < CanThrowTimeField(__instance))
            {
                return false;
            }

            AttackToolBinding? binding =
                ToolItemManager.GetAttackToolBinding(tool);

            if (binding == null)
            {
                return false;
            }

            customTool.OnUsed();

            ToolItemManager.ReportBoundAttackToolUsed(binding.Value);
            ToolItemManager.ReportBoundAttackToolUpdated(binding.Value);

            WillThrowToolField(__instance) = null;

            return false;
        }
    }
}
