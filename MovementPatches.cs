using HarmonyLib;
using SilksongRandomizer;

namespace SilksongAPExtra
{
    internal static class MovementPatches
    {
        [HarmonyPatch(typeof(HeroController), "BackOnGround")]
        private static class HeroControllerBackOnGroundPatch
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Player landed");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnLanded();
                }
            }
        }

        [HarmonyPatch(
            typeof(HeroController),
            nameof(HeroController.StartHarpoonDashCooldown))]
        private static class HeroControllerStartHarpoonDashCooldownPatch
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Player used Clawline");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnClawlineUsed();
                }
            }
        }

        [HarmonyPatch(typeof(HeroController), "StartFloat")]
        private static class HeroControllerStartFloatPatch
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Player started Drifter");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnDrifterStart();
                }
            }
        }

        [HarmonyPatch(typeof(HeroController), "FixedUpdate")]
        private static class HeroControllerFixedUpdatePatch
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnFixedUpdate();
                }
            }
        }

        // TODO: Add OnTransitionFinished
    }
}