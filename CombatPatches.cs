using GlobalEnums;
using HarmonyLib;
using SilksongRandomizer;
using System;
using UnityEngine;

namespace SilksongAPExtra
{
    internal static class CombatPatches
    {
        private static bool IsEnvironmentalHazard(HazardType hazardType)
        {
            return hazardType != HazardType.ENEMY && hazardType != HazardType.NON_HAZARD;
        }

        [HarmonyPatch(
            typeof(HeroController),
            nameof(HeroController.TakeDamage),
            new[]
            {
                typeof(GameObject),
                typeof(CollisionSide),
                typeof(int),
                typeof(HazardType),
                typeof(DamagePropertyFlags)
            })]
        private static class HeroControllerTakeDamagePatch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                HeroController __instance,
                GameObject go,
                int damageAmount,
                HazardType hazardType)
            {
                if (__instance.CanTakeDamage())
                {
                    bool shouldContinue = true;

                    // Damaged by Enemy
                    if (hazardType == HazardType.ENEMY)
                    {
                        // Prevent things such as DamageSelf() from counting
                        // as the player being hit by an enemy.
                        if (go != __instance.gameObject)
                        {
                            SilksongAPExtraPlugin.Instance.Log.LogInfo("Player hit by enemy");
                            foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                            {
                                tool.OnPlayerHitByEnemy(ref damageAmount, ref shouldContinue, ref go);
                                if (!shouldContinue) return false;
                            }
                        }
                    }
                    // Damaged by Hazard
                    else if (IsEnvironmentalHazard(hazardType))
                    {
                        SilksongAPExtraPlugin.Instance.Log.LogInfo("Player hit by hazard");
                        foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                        {
                            tool.OnPlayerHitByHazard(ref damageAmount, ref hazardType, ref shouldContinue, ref go);
                            if (!shouldContinue) return false;
                        }
                    }

                    // Generic damage
                    SilksongAPExtraPlugin.Instance.Log.LogInfo("Player damaged");
                    foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                    {
                        tool.OnPlayerDamaged(ref damageAmount, ref hazardType, ref shouldContinue, ref go);
                        if (!shouldContinue) return false;
                    }

                    return true;
                }
                return true;
            }
        }

        [HarmonyPatch(
            typeof(HealthManager),
            "TakeDamage",
            new[] { typeof(HitInstance) })]
        private static class HealthManagerTakeDamagePatch
        {
            [HarmonyPrefix]
            private static void Prefix(
                HealthManager __instance,
                HitInstance hitInstance,
                out int __state)
            {
                __state = -1;

                if (!hitInstance.IsHeroDamage)
                    return;

                HealthManager target = __instance.SendDamageTo ?? __instance;

                EnemyDeathEffects deathEffects =
                    target.GetComponent<EnemyDeathEffects>();

                if (deathEffects == null ||
                    deathEffects is EnemyDeathEffectsNoEffect)
                    return;

                if (hitInstance.IsHeroDamage)
                {
                    SilksongAPExtraPlugin.Instance.Log.LogInfo("Enemy pre damaged by player");
                    foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                    {
                        bool shouldContinue = true;
                        tool.OnEnemyPreDamagedByPlayer(hitInstance, __instance, ref shouldContinue);
                        if (!shouldContinue) break;
                    }
                }

                __state = target.hp;
            }

            [HarmonyPostfix]
            private static void Postfix(
                HealthManager __instance,
                HitInstance hitInstance,
                int __state)
            {
                if (__state < 0)
                    return;

                HealthManager target = __instance.SendDamageTo ?? __instance;

                if (target.hp < __state && hitInstance.IsHeroDamage)
                {
                    SilksongAPExtraPlugin.Instance.Log.LogInfo("Enemy damaged by player");
                    foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                    {
                        bool shouldContinue = true;
                        tool.OnEnemyDamagedByPlayer(hitInstance, __instance, target.hp - __state, ref shouldContinue);
                        if (!shouldContinue) break;
                    }
                }
            }
        }
    }
}