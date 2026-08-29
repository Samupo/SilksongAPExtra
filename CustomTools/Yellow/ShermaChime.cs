using SilksongAPExtra.Unity;
using SilksongRandomizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Yellow
{
    internal class ShermaChime : CustomToolItem
    {
        float targetTime = 0f;
        float currentTime = 0f;
        bool isPlaying = false;
        bool hasSpawnedFly = false;

        Sprite silkfly0 = null;
        Sprite silkfly1 = null;
        Sprite silkfly2 = null;
        Sprite silkfly3 = null;
        Sprite silkfly4 = null;

        public override void OnUpdate()
        {
            if (isPlaying)
            {
                currentTime += Time.deltaTime;
                if (currentTime > targetTime && !hasSpawnedFly)
                {
                    hasSpawnedFly = true;
                    SpawnSilkfly();
                }
            }
        }

        public override void OnNeedolinStart()
        {
            isPlaying = true;
            hasSpawnedFly = false;
            currentTime = 0f;
            targetTime = UnityEngine.Random.Range(5f, 10f);
        }

        public override void OnNeedolinEnd()
        {
            isPlaying = false;
        }

        public override void OnTransitionFinished()
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.05f)
            {
                SilksongAPExtraPlugin.Instance.StartCoroutine(SpawnOnTransitionCoroutine());
            }
        }

        IEnumerator SpawnOnTransitionCoroutine()
        {
            yield return new WaitForSeconds(3.0f);
            SpawnSilkfly();
        }

        public void SpawnSilkfly()
        {
            if (silkfly0 == null) silkfly0 = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "Silkfly0.png");
            if (silkfly1 == null) silkfly1 = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "Silkfly1.png");
            if (silkfly2 == null) silkfly2 = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "Silkfly2.png");
            if (silkfly3 == null) silkfly3 = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "Silkfly3.png");
            if (silkfly4 == null) silkfly4 = Utils.LoadSprite(SilksongAPExtraPlugin.Instance, "Silkfly4.png");

            GameObject go = new GameObject("Silkfly");
            var transform = go.transform;
            go.transform.position = HeroController.instance.gameObject.transform.position;
            var renderer = go.AddComponent<SpriteRenderer>();
            var anim = go.AddComponent<SimpleSpriteAnimation>();
            anim.Initialize(16.0f, silkfly0, silkfly1, silkfly2, silkfly3, silkfly3, silkfly4);
            go.AddComponent<ShermaChimeSilkflyStart>();
        }
    }

    public class ShermaChimeSilkflyStart : MonoBehaviour
    {
        IEnumerator Start()
        {
            const float duration = 3.0f;
            const float fadeDuration = 1.0f;
            const float radiusX = 1.5f;
            const float radiusY = 0.65f;
            const float radiusZ = 0.5f;
            const float flutterAmount = 0.18f;
            const float flutterSpeed = 18.0f;
            const float rotations = 1.5f;

            Vector3 startPosition = transform.position;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            float time = 0.0f;

            if (renderer != null)
            {
                Color color = renderer.color;
                color.a = 0.0f;
                renderer.color = color;
            }

            float startAngle = Mathf.PI;

            while (time < duration)
            {
                time += Time.deltaTime;

                float progress = Mathf.Clamp01(time / duration);
                float easedProgress = Mathf.SmoothStep(0.0f, 1.0f, progress);

                float angle = startAngle + easedProgress * Mathf.PI * 2.0f * rotations;

                float x = Mathf.Cos(angle) * radiusX;
                float y = Mathf.Sin(angle) * radiusY;
                float z = Mathf.Sin(angle) * radiusZ;

                float flutter = Mathf.Sin(time * flutterSpeed) * flutterAmount * Mathf.Sin(progress * Mathf.PI);

                transform.position = startPosition + new Vector3(x, y + flutter, z);

                if (renderer != null)
                {
                    Color color = renderer.color;
                    color.a = Mathf.Clamp01(time / fadeDuration);
                    renderer.color = color;
                }

                yield return null;
            }

            Destroy(this);
            gameObject.AddComponent<ShermaChimeSilkfly>();
        }
    }

    public class ShermaChimeSilkfly : MonoBehaviour
    {
        const float lifetimeDuration = 10.0f;
        const float fadeDuration = 1.0f;

        Vector2 direction;
        float lifetime = 0.0f;
        SpriteRenderer renderer;

        void Start()
        {
            renderer = GetComponent<SpriteRenderer>();

            foreach (var map in GameObject.FindObjectsByType<GameMap>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.InstanceID))
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo(map.gameObject.name);
            }

            if (Utils.TryGetClosestReachableCheck(
                GameObject.FindAnyObjectByType<GameMap>(),
                out Vector2 offset,
                out bool sameScene,
                out string locationName))
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo("Closest location found: " + locationName);

                direction = offset.normalized;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            lifetime += Time.fixedDeltaTime;

            transform.position += (Vector3)direction * Time.fixedDeltaTime * 6.0f;
            transform.position += Vector3.up * Mathf.Sin(lifetime) * 0.8f * Time.fixedDeltaTime;
            transform.position += Vector3.right * Mathf.Cos(lifetime * 0.4f) * 0.3f * Time.fixedDeltaTime;

            // Fade out over the final second.
            if (lifetime >= lifetimeDuration - fadeDuration)
            {
                float remaining = lifetimeDuration - lifetime;

                Color color = renderer.color;
                color.a = Mathf.Clamp01(remaining / fadeDuration);
                renderer.color = color;
            }

            if (lifetime >= lifetimeDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
