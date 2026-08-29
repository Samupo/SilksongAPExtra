using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Blue
{
    internal class SnailBadge : CustomToolItem
    {
        float timeSinceLastHit = 0f;
        private static readonly AccessTools.FieldRef<HeroController, GameObject> SpawnedLuckyDiceShieldEffect = AccessTools.FieldRefAccess<HeroController, GameObject>("spawnedLuckyDiceShieldEffect");
        private static readonly AccessTools.FieldRef<HeroController, float> ParryInvulnTimer = AccessTools.FieldRefAccess<HeroController, float>("parryInvulnTimer");

        public override void OnUpdate()
        {
            timeSinceLastHit += Time.deltaTime;
        }

        public override void OnPlayerHitByEnemy(ref int damageAmount, ref bool shouldContinue, ref GameObject go)
        {
            if (damageAmount > 0 && timeSinceLastHit > 60.0f)
            {
                damageAmount = 0;
                SpawnedLuckyDiceShieldEffect(HeroController.instance).SetActive(false);
                SpawnedLuckyDiceShieldEffect(HeroController.instance).SetActive(true);
                ParryInvulnTimer(HeroController.instance) = 0.5f;
                shouldContinue = false;
            }
            timeSinceLastHit = 0f;
        }
    }
}
