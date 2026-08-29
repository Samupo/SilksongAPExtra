using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilksongAPExtra.CustomTools.Yellow
{
    internal class ShakraBeads : CustomToolItem
    {
        float elapsedTime = 0f;

        public override void OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 12.0f)
            {
                elapsedTime = -3600f; // Just in case
                PlayerData.instance.scenesMapped.Add(GameManager.instance.sceneName);
                GameObject.FindAnyObjectByType<GameMap>().SetupMap(false);
            }
        }

        public override void OnTransitionFinished()
        {
            elapsedTime = 0f;
        }
    }
}
