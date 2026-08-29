using HarmonyLib;
using SilksongRandomizer;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Blue
{
    internal class DrifterWings : CustomToolItem
    {
        private static readonly AccessTools.FieldRef<HeroController, InputHandler> InputHandlerField =
            AccessTools.FieldRefAccess<HeroController, InputHandler>("inputHandler");

        private float driftTime = 0f;
        private bool drifting = false;
        private bool hasLanded = false;

        public override void OnLanded()
        {
            hasLanded = true;
            drifting = false;
        }

        public override void OnDrifterStart()
        {
            if (!hasLanded)
                return;

            drifting = true;
            driftTime = 0f;
            hasLanded = false;
        }

        public override void OnFixedUpdate()
        {
            if (!drifting)
                return;

            InputHandler inputHandler = InputHandlerField(HeroController.instance);

            // Can't use cState floating, so this will cover it for now
            if (inputHandler == null || !inputHandler.inputActions.Jump.IsPressed)
            {
                drifting = false;
                return;
            }

            driftTime += Time.fixedDeltaTime;

            if (driftTime >= 1.0f)
            {
                drifting = false;
                return;
            }

            Vector2 velocity = HeroController.instance.Body.linearVelocity;

            velocity.y = driftTime * 4.0f;

            HeroController.instance.Body.linearVelocity = velocity;
        }
    }
}