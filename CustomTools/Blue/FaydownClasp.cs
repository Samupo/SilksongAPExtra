using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Blue
{
    internal class FaydownClasp : CustomToolItem
    {
        bool canResetDoubleJump = false;

        private static readonly AccessTools.FieldRef<HeroController, bool> DoubleJumpedField = AccessTools.FieldRefAccess<HeroController, bool>("doubleJumped");

        public override void OnLanded()
        {
            canResetDoubleJump = true;
        }

        public override void OnClawlineUsed()
        {
            if (canResetDoubleJump)
            {
                canResetDoubleJump = false;
                DoubleJumpedField(HeroController.instance) = false;
            }
        }
    }
}
