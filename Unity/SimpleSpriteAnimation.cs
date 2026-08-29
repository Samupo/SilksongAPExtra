using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.Unity
{
    internal class SimpleSpriteAnimation : MonoBehaviour
    {
        float animationInterval = 0f;
        int currentFrame = 0;
        Sprite[] frames;
        SpriteRenderer renderer;
        float frameTime = 0f;

        public void Initialize(float animationSpeed, params Sprite[] sprites)
        {
            renderer = GetComponent<SpriteRenderer>();
            animationInterval = 1.0f / animationSpeed;
            frames = sprites;
            renderer.sprite = frames[0];
        }

        public void Update()
        {
            if (renderer == null) { Debug.LogWarning("No renderer attached to " + gameObject.name); return; }

            frameTime += Time.deltaTime;
            if (frameTime >= animationInterval)
            {
                frameTime -= animationInterval;
                currentFrame++;
                renderer.sprite = frames[currentFrame % frames.Length];
            }
        }
    }
}
