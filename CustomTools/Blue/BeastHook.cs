using System;
using System.Collections.Generic;
using System.Text;

namespace SilksongAPExtra.CustomTools.Blue
{
    internal class BeastHook : CustomToolItem
    {
        float cooldown = 0f;

        public override void OnUpdate()
        {
            cooldown -= UnityEngine.Time.deltaTime;
        }

        public override void OnEnemyDamagedByPlayer(HitInstance hitInstance, HealthManager enemyHealth, int damageDone, ref bool shouldContinue)
        {
            if (hitInstance.IsNailDamage && cooldown <= 0f)
            {
                if (PlayerData.instance.health <= 2 && PlayerData.instance.silk >= 4)
                {
                    cooldown = 15.0f;
                    HeroController.instance.TakeSilk(4, SilkSpool.SilkTakeSource.ActiveUse);
                    HeroController.instance.AddHealth(1);
                }

            }
        }
    }
}
