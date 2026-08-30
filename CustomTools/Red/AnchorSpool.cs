using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Red
{
    internal class AnchorSpool : CustomToolItem
    {
        public override void OnUsed()
        {
            AnchorSpoolBehavior anchor = GameObject.FindAnyObjectByType<AnchorSpoolBehavior>();
            if (anchor == null)
            {
                GameObject anchorObject = new GameObject("Anchor Spool");
                anchorObject.transform.position = HeroController.instance.transform.position;
                anchorObject.AddComponent<AnchorSpoolBehavior>();
            }
            else
            {
                HeroController.instance.transform.position = anchor.transform.position;
                GameObject.Destroy(anchor.gameObject);
                DecreaseAmount(1);
            }
        }
    }

    public class AnchorSpoolBehavior : MonoBehaviour
    {

    }
}
