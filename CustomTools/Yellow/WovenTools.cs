using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Yellow
{
    internal class WovenTools : CustomToolItem
    {
        public override void OnEnemyPreDamagedByPlayer(HitInstance hitInstance, HealthManager enemyHealth,ref bool shouldContinue)
        {
            if (!hitInstance.IsNailDamage)
            {
                int increments = PlayerData.instance.silk / 9;
                int originalDamage = hitInstance.DamageDealt;
                hitInstance.DamageDealt += (int)MathF.Ceiling(hitInstance.DamageDealt * 0.15f * increments);

                SilksongAPExtraPlugin.Instance.Log.LogInfo("Woven Tools: bumped damage from " + originalDamage + " to " + hitInstance.DamageDealt);
            }
        }
    }
}
