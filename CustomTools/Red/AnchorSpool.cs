using SilksongRandomizer;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Red
{
    internal class AnchorSpool : CustomToolItem
    {
        const float TRANSITION_DURATION = 2.0f;

        Sprite anchorSprite;
        float transitionTime = 0f;
        bool transitioning = false;
        Vector3 initialPos;
        AnchorSpoolBehavior anchor;

        public AnchorSpool()
        {
            anchorSprite = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "AnchorSpoolSprite.png");
        }

        public override void OnUsed()
        {
            if (transitioning) return;

            anchor = GameObject.FindAnyObjectByType<AnchorSpoolBehavior>();
            if (anchor == null)
            {
                GameObject anchorObject = new GameObject("Anchor Spool");
                anchorObject.transform.position = HeroController.instance.transform.position +  Vector3.up * 0.5f;
                anchorObject.AddComponent<AnchorSpoolBehavior>();
                SpriteRenderer renderer = anchorObject.AddComponent<SpriteRenderer>();
                renderer.sprite = anchorSprite;
            }
            else
            {
                transitionTime = 0f;
                transitioning = true;
                initialPos = HeroController.instance.transform.position;
                DecreaseAmount(1);
            }
        }

        public override void OnUpdate()
        {
            if (transitioning)
            {
                HeroController.instance.acceptingInput = false;
                transitionTime += Time.deltaTime;
                HeroController.instance.Body.gravityScale = 0f;

                if (transitionTime > TRANSITION_DURATION)
                {
                    HeroController.instance.Body.gravityScale = 1f;
                    HeroController.instance.acceptingInput = true;
                    HeroController.instance.transform.position = anchor.transform.position;
                    GameObject.Destroy(anchor.gameObject);
                    transitioning = false;
                }
                else
                {
                    float a = transitionTime / TRANSITION_DURATION;
                    HeroController.instance.transform.position = Vector3.Lerp(initialPos, anchor.transform.position, a * a);
                }
            }
        }
    }

    public class AnchorSpoolBehavior : MonoBehaviour
    {
        Vector3 startPos;

        void Start()
        {
            startPos = this.transform.position;
        }

        void Update()
        {
            this.transform.position = startPos + Mathf.Sin(Time.time * 1.5f) * Vector3.up * 0.25f;
            this.transform.rotation = Quaternion.Euler(0f, 0f, Time.time * 20f);
        }
    }
}
