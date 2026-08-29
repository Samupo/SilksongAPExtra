using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Yellow
{
    internal class ArchitectSeal : CustomToolItem
    {
        public override void OnEnemyPreDamagedByPlayer(HitInstance hitInstance, HealthManager enemyHealth, ref bool shouldContinue)
        {
            int originalDamage = hitInstance.DamageDealt;
            if (!hitInstance.IsNailDamage)
            {
                hitInstance.DamageDealt = (int)MathF.Ceiling(hitInstance.DamageDealt * 1.5f);
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Architect Seal: bumped damage from " + originalDamage + " to " + hitInstance.DamageDealt);
            }
            else
            {
                hitInstance.DamageDealt = (int)MathF.Ceiling(hitInstance.DamageDealt * 0.5f);
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Architect Seal: reduced damage from " + originalDamage + " to " + hitInstance.DamageDealt);
            }
        }
    }
}
