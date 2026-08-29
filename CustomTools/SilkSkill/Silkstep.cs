using HarmonyLib;
using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.SilkSkill
{
    internal class Silkstep : CustomToolItem
    {
        private static readonly AccessTools.FieldRef<HeroController, bool> DoubleJumpedField = AccessTools.FieldRefAccess<HeroController, bool>("doubleJumped");
        private static readonly AccessTools.FieldRef<HeroController, bool> AirDashedField = AccessTools.FieldRefAccess<HeroController, bool>("airDashed");

        float jumpTime = 0f;

        public override void OnUsed()
        {
            HeroController.instance.TakeSilk(4, SilkSpool.SilkTakeSource.ActiveUse);
            DoubleJumpedField(HeroController.instance) = false;
            AirDashedField(HeroController.instance) = false;
            HeroController.instance.doubleJumpEffectPrefab.Spawn(HeroController.instance.transform);
            jumpTime = 0.125f;
        }

        public override void OnFixedUpdate()
        {
            if (jumpTime > 0)
            {
                jumpTime -= Time.fixedDeltaTime;
                Vector2 velocity = HeroController.instance.Body.linearVelocity;
                velocity.y = 8.0f;
                HeroController.instance.Body.linearVelocity = velocity;
            }
        }
    }
}
