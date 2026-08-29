using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.SilkSkill
{
    internal class Stillthread : CustomToolItem
    {
        float activeTime = 0f;
        bool active = false;

        public override void OnUpdate()
        {
            if (active)
            {
                activeTime += Time.unscaledDeltaTime;
                if (activeTime < 12.0f)
                {
                    if (Time.timeScale == 1.0f) Time.timeScale = 0.8f;
                }
                else
                {
                    if (Time.timeScale == 0.8f) Time.timeScale = 1.0f;
                    active = false;
                }
            }
        }

        public override void OnUsed()
        {
            HeroController.instance.TakeSilk(4, SilkSpool.SilkTakeSource.ActiveUse);
            active = true;
            activeTime = 0f;
        }
    }
}
