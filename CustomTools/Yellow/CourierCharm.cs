using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Yellow
{
    internal class CourierCharm : CustomToolItem
    {
        float time = 0f;

        public override void OnUpdate()
        {
            time += Time.deltaTime;
            if (time >= 60.0f)
            {
                time -= 60.0f;
                bool active = false;
                foreach (var item in DeliveryQuestItem.GetActiveItems())
                {
                    if (item.CurrentCount < item.MaxCount)
                    {
                        active = true;
                        item.Item.Get(1, false);
                    }
                }
                if (active) EventRegister.SendEvent(EventRegisterEvents.DeliveryHudRefresh, null);
            }
        }
    }
}
