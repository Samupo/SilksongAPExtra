using GlobalEnums;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Blue
{
    internal class LifelineSpool : CustomToolItem
    {
        public override void OnPlayerDamaged(ref int damageAmount, ref HazardType hazardType, ref bool shouldContinue, ref GameObject go)
        {
            if (damageAmount > 1 && PlayerData.instance.silk >= 3)
            {
                damageAmount = 1;
                HeroController.instance.TakeSilk(3, SilkSpool.SilkTakeSource.ActiveUse);
            }
        }
    }
}
