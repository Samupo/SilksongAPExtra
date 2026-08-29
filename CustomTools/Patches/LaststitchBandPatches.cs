using System;
using HarmonyLib;
using HutongGames.PlayMaker;
using SilksongAPExtra.CustomTools;

[HarmonyPatch(typeof(HeroController), "Start")]
public static class LaststitchBandPatches
{
    private const string QuickBindStateName = "Quick Bind?";
    private const string BindTimeVariableName = "Bind Time";

    private const float LaststitchMultiplier = 0.3f;

    [HarmonyPostfix]
    private static void HeroControllerStartPostfix(HeroController __instance)
    {
        if (__instance == null)
        {
            return;
        }

        /*
         * Do NOT search all PlayMakerFSMs.
         *
         * HeroController itself resolves the bind/spell FSM and stores it in
         * spellControl. This also avoids depending on whether its GameObject
         * hierarchy is Hero_Hornet/Bind or something else.
         */
        PlayMakerFSM fsm = __instance.spellControl;

        if (fsm == null)
        {
            return;
        }

        FsmFloat bindTime =
            fsm.FsmVariables.GetFsmFloat(BindTimeVariableName);

        if (bindTime == null)
        {
            return;
        }

        FsmState quickBindState =
            FindState(fsm, QuickBindStateName);

        if (quickBindState == null)
        {
            return;
        }

        /*
         * Structural safety check.
         *
         * We are already restricted to HeroController.spellControl, but also
         * require the exact state and variable found in the Bind FSM dump.
         *
         * This prevents this code from modifying arbitrary PlayMaker FSMs.
         */

        FsmStateAction[] oldActions = quickBindState.Actions;

        if (oldActions == null)
        {
            oldActions = Array.Empty<FsmStateAction>();
        }

        /*
         * HeroController.Start should normally only initialize this once,
         * but guard against duplicate injection anyway.
         */
        for (int i = 0; i < oldActions.Length; i++)
        {
            if (oldActions[i] is LaststitchBindMultiplierAction)
            {
                return;
            }
        }

        var action = new LaststitchBindMultiplierAction
        {
            BindTime = bindTime
        };

        /*
         * Insert at action index 0.
         *
         * This is important.
         *
         * Vanilla "Quick Bind?" is:
         *
         *   CheckIfToolEquipped Quickbind
         *       false -> FINISHED
         *
         *   FloatMultiply
         *       Bind Time *= 0.6
         *
         * If our action were inserted after CheckIfToolEquipped, it would
         * never execute when Injector Band was unequipped.
         *
         * By inserting first:
         *
         *   Laststitch active + 1/2 HP:
         *       Bind Time *= 0.3
         *
         *   then vanilla Quickbind check:
         *
         *   Injector equipped:
         *       Bind Time *= 0.6
         *
         * Therefore both naturally stack:
         *
         *       Bind Time *= 0.3 * 0.6
         *                 *= 0.18
         */
        FsmStateAction[] newActions =
            new FsmStateAction[oldActions.Length + 1];

        newActions[0] = action;

        Array.Copy(
            oldActions,
            0,
            newActions,
            1,
            oldActions.Length);

        quickBindState.Actions = newActions;

        /*
         * The FSM has already been initialized by this point, so initialize
         * the newly inserted action against its owning state.
         */
        action.Init(quickBindState);
    }

    private static FsmState FindState(
        PlayMakerFSM fsm,
        string stateName)
    {
        FsmState[] states = fsm.FsmStates;

        if (states == null)
        {
            return null;
        }

        for (int i = 0; i < states.Length; i++)
        {
            FsmState state = states[i];

            if (state != null &&
                string.Equals(
                    state.Name,
                    stateName,
                    StringComparison.Ordinal))
            {
                return state;
            }
        }

        return null;
    }

    private sealed class LaststitchBindMultiplierAction
        : FsmStateAction
    {
        public FsmFloat BindTime;

        public override void OnEnter()
        {
            try
            {
                if (!CustomTools.laststitchBand.IsEquipped)
                {
                    return;
                }

                HeroController hero = HeroController.instance;

                if (hero == null || hero.playerData == null)
                {
                    return;
                }

                int health = hero.playerData.health;

                /*
                 * Exactly 1 or 2 masks.
                 *
                 * Do not apply at 0 HP and do not apply at 3+ HP.
                 */
                if (health > 2)
                {
                    return;
                }

                if (BindTime == null)
                {
                    return;
                }

                BindTime.Value *= LaststitchMultiplier;
            }
            finally
            {
                /*
                 * This action is instantaneous. It must not hold the FSM
                 * in this action after applying/checking the multiplier.
                 */
                Finish();
            }
        }
    }
}