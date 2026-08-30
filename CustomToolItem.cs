using BepInEx;
using GlobalEnums;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TeamCherry.Localization;
using UnityEngine;
using static ToolItem;

namespace SilksongAPExtra
{
    public struct CustomToolItemData
    {
        public string userFriendlyName;
        public string toolNameID;
        public string toolDescriptionID;
        public ToolItemType type;
        public string spritePath;
        public UsageOptions usage;
        public int redToolBaseCount;
    }

    public class CustomToolItem : ToolItem
    {
        private BaseUnityPlugin source;
        private CustomToolItemData data;
        private Sprite inventorySprite = null;
        private Sprite inventoryPoisonedSprite = null;
        private UsageOptions usage;

        public override UsageOptions Usage => usage;
        public override LocalisedString DisplayName => new LocalisedString("Mods." + source.Info.Metadata.GUID, data.toolNameID);
        public override LocalisedString Description => new LocalisedString("Mods." + source.Info.Metadata.GUID, data.toolDescriptionID);

        #region Harmony accessors
        private static readonly AccessTools.FieldRef<ToolItem, ToolItemType> TypeField = AccessTools.FieldRefAccess<ToolItem, ToolItemType>("type");
        private static readonly AccessTools.FieldRef<ToolItem, PlayerDataTest> AlternateUnlockedTestField = AccessTools.FieldRefAccess<ToolItem, PlayerDataTest>("alternateUnlockedTest");
        private static readonly AccessTools.FieldRef<ToolItem, int> BaseStorageAmountField = AccessTools.FieldRefAccess<ToolItem, int>("baseStorageAmount");

        public ToolItemType ToolType
        {
            get => TypeField(this);
            set => TypeField(this) = value;
        }
        #endregion

        public static T CreateTool<T>(BaseUnityPlugin source, CustomToolItemData data) where T : CustomToolItem
        {
            T tool = ScriptableObject.CreateInstance<T>();
            tool.source = source;
            tool.data = data;
            tool.name = data.userFriendlyName;
            tool.ToolType = data.type;
            tool.usage = data.usage;

            if (!string.IsNullOrEmpty(data.spritePath))
            {
                tool.inventorySprite = Utils.LoadSprite(source, data.spritePath);
            }
            // Try load placeholder
            if (tool.inventorySprite == null)
            {
                tool.inventorySprite = Utils.LoadSprite(source, "Placeholder.png");
            }

            if (data.type == ToolItemType.Red)
            {
                BaseStorageAmountField(tool) = data.redToolBaseCount;
            }

            AlternateUnlockedTestField(tool) = new PlayerDataTest();
            return tool;
        }

        public override Sprite GetInventorySprite(IconVariants iconVariant)
        {
            if (iconVariant == IconVariants.Poison && inventoryPoisonedSprite != null) return inventoryPoisonedSprite;
            return inventorySprite;
        }

        public override Sprite GetHudSprite(IconVariants iconVariant)
        {
            return null;
        }

        public virtual void DecreaseAmount(int amount)
        {
            var saveData = this.SavedData;
            saveData.AmountLeft -= amount;
            this.SavedData = saveData;
        }

        public virtual void OnPlayerHitByEnemy(ref int damageAmount, ref bool shouldContinue, ref GameObject go) { }
        public virtual void OnPlayerHitByHazard(ref int damageAmount, ref HazardType hazardType, ref bool shouldContinue, ref GameObject go) { }
        public virtual void OnPlayerDamaged(ref int damageAmount, ref HazardType hazardType, ref bool shouldContinue, ref GameObject go) { }

        public virtual void OnEnemyPreDamagedByPlayer(HitInstance hitInstance, HealthManager enemyHealth, ref bool shouldContinue) { }
        public virtual void OnEnemyDamagedByPlayer(HitInstance hitInstance, HealthManager enemyHealth, int damageDone, ref bool shouldContinue) { }

        public virtual void OnLanded() { }
        public virtual void OnClawlineUsed() { }
        public virtual void OnDrifterStart() { }

        public virtual void OnUsed() { }
        public virtual void OnUsageFailed() { } // Not enough silk/amount left

        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }

        // Not implemented
        //public virtual void OnEquip() { }
        //public virtual void OnUnequip() { }

        public virtual void OnNeedolinStart() { }
        public virtual void OnNeedolinEnd() { }

        public virtual void OnTransitionFinished() { }
    }
}
