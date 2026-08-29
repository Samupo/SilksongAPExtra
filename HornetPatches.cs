using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SilksongRandomizer;
using System.Collections.Generic;

namespace SilksongAPExtra
{
    internal static class HornetPatches
    {
        private const string StartNeedolinState = "Start Needolin Proper";

        private static readonly HashSet<string> EndNeedolinStates = new HashSet<string>
        {
            "Break Loop",
            "Cancel Needolin?",
            "Pass CANCEL",
            "End Needolin",
            "Needolin Lock",
            "Needolin Lock 2"
        };

        private static bool isNeedolinPlaying;
        private static Fsm activeNeedolinFsm;

        [HarmonyPatch(typeof(StartNeedolinAudioLoop), nameof(StartNeedolinAudioLoop.OnEnter))]
        private static class StartNeedolinAudioLoopOnEnterPatch
        {
            [HarmonyPostfix]
            private static void Postfix(StartNeedolinAudioLoop __instance)
            {
                if (__instance.State == null ||
                    __instance.State.Name != StartNeedolinState ||
                    isNeedolinPlaying)
                {
                    return;
                }

                isNeedolinPlaying = true;
                activeNeedolinFsm = __instance.Fsm;

                SilksongAPExtraPlugin.Instance.Log.LogInfo("Player started Needolin");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnNeedolinStart();
                }
            }
        }

        [HarmonyPatch(typeof(Fsm), "EnterState", new[] { typeof(FsmState) })]
        private static class FsmEnterStatePatch
        {
            [HarmonyPrefix]
            private static void Prefix(Fsm __instance, FsmState state)
            {
                if (!isNeedolinPlaying ||
                    state == null ||
                    !object.ReferenceEquals(__instance, activeNeedolinFsm) ||
                    !EndNeedolinStates.Contains(state.Name))
                {
                    return;
                }

                isNeedolinPlaying = false;
                activeNeedolinFsm = null;

                SilksongAPExtraPlugin.Instance.Log.LogInfo("Player stopped Needolin");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnNeedolinEnd();
                }
            }
        }
    }
}