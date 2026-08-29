using BepInEx;
using SilksongRandomizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilksongAPExtra
{
    internal class Utils
    {
        public static Sprite LoadSprite(BaseUnityPlugin plugin, string fileName)
        {
            string modFolder = Path.GetDirectoryName(plugin.Info.Location);
            string path = Path.Combine(modFolder, "Sprites", fileName);

            if (!File.Exists(path))
            {
                Debug.LogError($"Sprite file not found: {path}");
                return null;
            }

            byte[] imageData = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2);

            if (!texture.LoadImage(imageData))
            {
                Debug.LogError($"Failed to load sprite: {path}");
                UnityEngine.Object.Destroy(texture);
                return null;
            }

            texture.name = Path.GetFileNameWithoutExtension(fileName);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );

            sprite.name = texture.name;

            return sprite;
        }

        internal static bool TryGetClosestReachableCheck(GameMap map, out Vector2 offset, out bool sameScene, out string locationName)
        {
            offset = default;
            sameScene = false;
            locationName = null;

            if (map == null)
                return false;

            Assembly randomizer = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(x => x.GetName().Name == "SilksongRandomizer");

            Type manager = randomizer?.GetType(
                "SilksongRandomizer.Patches.CheckMapMarkerManager");

            if (manager == null)
                return false;

            const BindingFlags StaticFlags =
                BindingFlags.Static | BindingFlags.NonPublic;

            const BindingFlags InstanceFlags =
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic;

            manager.GetMethod("Refresh", StaticFlags)
                ?.Invoke(null, new object[] { map });

            IDictionary markers = manager
                .GetField("MarkersByLocation", StaticFlags)
                ?.GetValue(null) as IDictionary;

            IDictionary logic = manager
                .GetField("ReachabilityByLocation", StaticFlags)
                ?.GetValue(null) as IDictionary;

            if (markers == null || logic == null)
                return false;

            if (!TryGetHornetMapPosition(
                    map,
                    out Vector2 hornetPosition,
                    out GameMapScene hornetScene))
            {
                return false;
            }

            float bestDistance = float.PositiveInfinity;

            foreach (DictionaryEntry entry in markers)
            {
                object reachability = logic[entry.Key];

                if (reachability == null ||
                    reachability.ToString() != "Reachable")
                {
                    continue;
                }

                if (!(entry.Value is IEnumerable markerList))
                    continue;

                foreach (object marker in markerList)
                {
                    if (marker == null)
                        continue;

                    Type markerType = marker.GetType();

                    object positionValue = markerType
                        .GetField("MapPosition", InstanceFlags)
                        ?.GetValue(marker);

                    if (!(positionValue is Vector2 checkPosition))
                        continue;

                    Vector2 candidateOffset =
                        checkPosition - hornetPosition;

                    float distance =
                        candidateOffset.sqrMagnitude;

                    if (distance >= bestDistance)
                        continue;

                    GameMapScene checkScene = markerType
                        .GetField("Scene", InstanceFlags)
                        ?.GetValue(marker) as GameMapScene;

                    bestDistance = distance;
                    offset = candidateOffset;
                    locationName = entry.Key?.ToString();
                    sameScene = checkScene == hornetScene;
                }
            }

            return bestDistance < float.PositiveInfinity;
        }

        private static bool TryGetHornetMapPosition(
            GameMap map,
            out Vector2 mapPosition,
            out GameMapScene mapScene)
        {
            mapPosition = default;
            mapScene = null;

            HeroController hero = HeroController.instance;
            GameManager gameManager = GameManager.instance;

            if (map == null ||
                hero == null ||
                gameManager == null)
            {
                return false;
            }

            string sceneName = SceneManager.GetActiveScene().name;

            mapScene = map
                .GetComponentsInChildren<GameMapScene>(true)
                .FirstOrDefault(scene =>
                    scene != null &&
                    string.Equals(
                        scene.Name,
                        sceneName,
                        StringComparison.OrdinalIgnoreCase));

            if (mapScene == null ||
                mapScene.transform.parent == null)
            {
                return false;
            }

            Vector3 sceneLocal =
                mapScene.transform.localPosition;

            Vector3 parentLocal =
                mapScene.transform.parent.localPosition;

            Vector2 roomMapCenter = new Vector2(
                sceneLocal.x + parentLocal.x,
                sceneLocal.y + parentLocal.y);

            // This matches the randomizer's fallback for rooms
            // without a BoundsSprite.
            if (mapScene.BoundsSprite == null)
            {
                mapPosition = roomMapCenter;
                return true;
            }

            const BindingFlags Flags =
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic;

            FieldInfo sceneWidthField =
                typeof(GameManager).GetField(
                    "sceneWidth",
                    Flags);

            FieldInfo sceneHeightField =
                typeof(GameManager).GetField(
                    "sceneHeight",
                    Flags);

            if (sceneWidthField == null ||
                sceneHeightField == null)
            {
                return false;
            }

            float sceneWidth = Convert.ToSingle(
                sceneWidthField.GetValue(gameManager));

            float sceneHeight = Convert.ToSingle(
                sceneHeightField.GetValue(gameManager));

            if (sceneWidth <= 0f ||
                sceneHeight <= 0f)
            {
                return false;
            }

            Vector2 roomMapSize =
                (Vector2)mapScene.BoundsSprite.bounds.size *
                (Vector2)mapScene.transform.localScale;

            Vector3 heroPosition =
                hero.transform.position;

            mapPosition = new Vector2(
                roomMapCenter.x -
                    roomMapSize.x * 0.5f +
                    heroPosition.x / sceneWidth * roomMapSize.x,

                roomMapCenter.y -
                    roomMapSize.y * 0.5f +
                    heroPosition.y / sceneHeight * roomMapSize.y);

            return
                !float.IsNaN(mapPosition.x) &&
                !float.IsInfinity(mapPosition.x) &&
                !float.IsNaN(mapPosition.y) &&
                !float.IsInfinity(mapPosition.y);
        }
    }
}
