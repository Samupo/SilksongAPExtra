using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SilksongAPExtra;
using SilksongAPExtra.CustomTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilksongRandomizer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("moriko.silksong.randomizer", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("org.silksong-modding.i18n")]
    public class SilksongAPExtraPlugin : BaseUnityPlugin
    {
        public static SilksongAPExtraPlugin Instance { get; private set; }
        public const string PluginGuid = "samupo.silksong.apextra";
        public const string PluginName = "APExtra";
        public const string PluginVersion = "0.0.0";

        public new ManualLogSource Log => this.Logger;

        private void Awake()
        {
            Instance = this;

            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            // Check ingame
            if (PlayerData.instance != null)
            {
                SilksongAPExtraPlugin.Instance.Log.LogInfo("On Transition");
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    tool.OnTransitionFinished();
                }
            }
        }

        void Start()
        {
            CustomTools.Initialize(this);
        }

        void Update()
        {
            if (ToolItemManager.Instance != null)
            {
                CustomToolManager.TryPatchTools(ToolItemManager.Instance);
                foreach (CustomToolItem tool in CustomToolManager.GetEquippedTools())
                {
                    try
                    {
                        tool.OnUpdate();
                    }
                    catch (Exception ex)
                    {
                        Log.LogError(ex);
                    }
                }
            }
        }
    }
}
