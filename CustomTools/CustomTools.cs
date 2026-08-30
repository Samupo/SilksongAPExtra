using BepInEx;
using JetBrains.Annotations;
using SilksongAPExtra.CustomTools.Blue;
using SilksongAPExtra.CustomTools.Red;
using SilksongAPExtra.CustomTools.SilkSkill;
using SilksongAPExtra.CustomTools.Yellow;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static ToolItem;

namespace SilksongAPExtra.CustomTools
{
    internal static class CustomTools
    {
        // Red
        public static AnchorSpool anchorSpool;

        // Blue
        public static BeastHook beastHook;
        public static DrifterWings drifterWings;
        public static FaydownClasp faydownClasp;
        public static LaststitchBand laststitchBand;
        public static LifelineSpool lifelineSpool;
        public static Silkseed silkseed;
        public static SnailBadge snailBadge;

        // Yellow
        public static ArchitectSeal architectSeal;
        public static CourierCharm courierCharm;
        public static HagglerMask hagglerMask;
        public static ShakraBeads shakraBeads;
        public static ShermaChime shermaChime;
        public static WovenTools wovenTools;

        // Silk Skills
        public static Silkstep silkstep;
        public static Stillthread stillthread;

        public static void Initialize(BaseUnityPlugin plugin)
        {
            // Red
            CustomToolItemData anchorSpoolData = new CustomToolItemData();
            anchorSpoolData.type = ToolItemType.Red;
            anchorSpoolData.userFriendlyName = "Anchor Spool";
            anchorSpoolData.toolNameID = "ANCHOR_SPOOL";
            anchorSpoolData.toolDescriptionID = "ANCHOR_SPOOL_DESC";
            anchorSpoolData.spritePath = "AnchorSpool.png";
            anchorSpoolData.redToolBaseCount = 2;
            anchorSpoolData.usage = new UsageOptions();
            anchorSpool = CustomToolItem.CreateTool<AnchorSpool>(plugin, anchorSpoolData);
            CustomToolManager.RegisterTool(anchorSpool);

            // Blue
            CustomToolItemData beastHookData = new CustomToolItemData();
            beastHookData.type = ToolItemType.Blue;
            beastHookData.userFriendlyName = "Beast Hook";
            beastHookData.toolNameID = "BEAST_HOOK";
            beastHookData.toolDescriptionID = "BEAST_HOOK_DESC";
            beastHookData.spritePath = "BeastHook.png";
            beastHook = CustomToolItem.CreateTool<BeastHook>(plugin, beastHookData);
            CustomToolManager.RegisterTool(beastHook);

            CustomToolItemData drifterWingsData = new CustomToolItemData();
            drifterWingsData.type = ToolItemType.Blue;
            drifterWingsData.userFriendlyName = "Drifter Wings";
            drifterWingsData.toolNameID = "DRIFTER_WINGS";
            drifterWingsData.toolDescriptionID = "DRIFTER_WINGS_DESC";
            drifterWingsData.spritePath = "DrifterWings.png";
            drifterWings = CustomToolItem.CreateTool<DrifterWings>(plugin, drifterWingsData);
            CustomToolManager.RegisterTool(drifterWings);

            CustomToolItemData faydownClaspData = new CustomToolItemData();
            faydownClaspData.type = ToolItemType.Blue;
            faydownClaspData.userFriendlyName = "Faydown Clasp";
            faydownClaspData.toolNameID = "FAYDOWN_CLASP";
            faydownClaspData.toolDescriptionID = "FAYDOWN_CLASP_DESC";
            faydownClaspData.spritePath = "FaydownClasp.png";
            faydownClasp = CustomToolItem.CreateTool<FaydownClasp>(plugin, faydownClaspData);
            CustomToolManager.RegisterTool(faydownClasp);

            CustomToolItemData laststitchBandData = new CustomToolItemData();
            laststitchBandData.type = ToolItemType.Blue;
            laststitchBandData.userFriendlyName = "Laststitch Band";
            laststitchBandData.toolNameID = "LASTSTITCH_BAND";
            laststitchBandData.toolDescriptionID = "LASTSTITCH_BAND_DESC";
            laststitchBandData.spritePath = "LaststitchBand.png";
            laststitchBand = CustomToolItem.CreateTool<LaststitchBand>(plugin, laststitchBandData);
            CustomToolManager.RegisterTool(laststitchBand);

            CustomToolItemData lifelineSpoolData = new CustomToolItemData();
            lifelineSpoolData.type = ToolItemType.Blue;
            lifelineSpoolData.userFriendlyName = "Lifeline Spool";
            lifelineSpoolData.toolNameID = "LIFELINE_SPOOL";
            lifelineSpoolData.toolDescriptionID = "LIFELINE_SPOOL_DESC";
            lifelineSpoolData.spritePath = "LifelineSpool.png";
            lifelineSpool = CustomToolItem.CreateTool<LifelineSpool>(plugin, lifelineSpoolData);
            CustomToolManager.RegisterTool(lifelineSpool);

            CustomToolItemData silkseedData = new CustomToolItemData();
            silkseedData.type = ToolItemType.Blue;
            silkseedData.userFriendlyName = "Silkseed";
            silkseedData.toolNameID = "SILKSEED";
            silkseedData.toolDescriptionID = "SILKSEED_DESC";
            silkseedData.spritePath = "Silkseed.png";
            silkseed = CustomToolItem.CreateTool<Silkseed>(plugin, silkseedData);
            CustomToolManager.RegisterTool(silkseed);

            CustomToolItemData snailBadgeData = new CustomToolItemData();
            snailBadgeData.type = ToolItemType.Blue;
            snailBadgeData.userFriendlyName = "Snail Badge";
            snailBadgeData.toolNameID = "SNAIL_BADGE";
            snailBadgeData.toolDescriptionID = "SNAIL_BADGE_DESC";
            snailBadgeData.spritePath = "SnailBadge.png";
            snailBadge = CustomToolItem.CreateTool<SnailBadge>(plugin, snailBadgeData);
            CustomToolManager.RegisterTool(snailBadge);

            // Yellow
            CustomToolItemData architectSealData = new CustomToolItemData();
            architectSealData.type = ToolItemType.Yellow;
            architectSealData.userFriendlyName = "Architect Seal";
            architectSealData.toolNameID = "ARCHITECT_SEAL";
            architectSealData.toolDescriptionID = "ARCHITECT_SEAL_DESC";
            architectSealData.spritePath = "ArchitectSeal.png";
            architectSeal = CustomToolItem.CreateTool<ArchitectSeal>(plugin, architectSealData);
            CustomToolManager.RegisterTool(architectSeal);

            CustomToolItemData courierCharmData = new CustomToolItemData();
            courierCharmData.type = ToolItemType.Yellow;
            courierCharmData.userFriendlyName = "Courier Charm";
            courierCharmData.toolNameID = "COURIER_CHARM";
            courierCharmData.toolDescriptionID = "COURIER_CHARM_DESC";
            courierCharmData.spritePath = "CourierCharm.png";
            courierCharm = CustomToolItem.CreateTool<CourierCharm>(plugin, courierCharmData);
            CustomToolManager.RegisterTool(courierCharm);

            CustomToolItemData hagglerMaskData = new CustomToolItemData();
            hagglerMaskData.type = ToolItemType.Yellow;
            hagglerMaskData.userFriendlyName = "Haggler Mask";
            hagglerMaskData.toolNameID = "HAGGLER_MASK";
            hagglerMaskData.toolDescriptionID = "HAGGLER_MASK_DESC";
            hagglerMaskData.spritePath = "HagglerMask.png";
            hagglerMask = CustomToolItem.CreateTool<HagglerMask>(plugin, hagglerMaskData);
            CustomToolManager.RegisterTool(hagglerMask);

            CustomToolItemData shakraBeadsData = new CustomToolItemData();
            shakraBeadsData.type = ToolItemType.Yellow;
            shakraBeadsData.userFriendlyName = "Shakra Beads";
            shakraBeadsData.toolNameID = "SHAKRA_BEADS";
            shakraBeadsData.toolDescriptionID = "SHAKRA_BEADS_DESC";
            shakraBeadsData.spritePath = "ShakraBeads.png";
            shakraBeads = CustomToolItem.CreateTool<ShakraBeads>(plugin, shakraBeadsData);
            CustomToolManager.RegisterTool(shakraBeads);

            CustomToolItemData shermaChimeData = new CustomToolItemData();
            shermaChimeData.type = ToolItemType.Yellow;
            shermaChimeData.userFriendlyName = "Sherma Chime";
            shermaChimeData.toolNameID = "SHERMA_CHIME";
            shermaChimeData.toolDescriptionID = "SHERMA_CHIME_DESC";
            shermaChimeData.spritePath = "ShermaChime.png";
            shermaChime = CustomToolItem.CreateTool<ShermaChime>(plugin, shermaChimeData);
            CustomToolManager.RegisterTool(shermaChime);

            CustomToolItemData wovenToolsData = new CustomToolItemData();
            wovenToolsData.type = ToolItemType.Yellow;
            wovenToolsData.userFriendlyName = "Woven Tools";
            wovenToolsData.toolNameID = "WOVEN_TOOLS";
            wovenToolsData.toolDescriptionID = "WOVEN_TOOLS_DESC";
            wovenToolsData.spritePath = "WovenTools.png";
            wovenTools = CustomToolItem.CreateTool<WovenTools>(plugin, wovenToolsData);
            CustomToolManager.RegisterTool(wovenTools);

            // Silk Skills
            CustomToolItemData silkstepData = new CustomToolItemData();
            silkstepData.type = ToolItemType.Skill;
            silkstepData.userFriendlyName = "Silkstep";
            silkstepData.toolNameID = "SILKSTEP";
            silkstepData.toolDescriptionID = "SILKSTEP_DESC";
            silkstepData.spritePath = "Silkstep.png";
            silkstep = CustomToolItem.CreateTool<Silkstep>(plugin, silkstepData);
            CustomToolManager.RegisterTool(silkstep);

            CustomToolItemData stillthreadData = new CustomToolItemData();
            stillthreadData.type = ToolItemType.Skill;
            stillthreadData.userFriendlyName = "Stillthread";
            stillthreadData.toolNameID = "STILLTHREAD";
            stillthreadData.toolDescriptionID = "STILLTHREAD_DESC";
            stillthreadData.spritePath = "Stillthread.png";
            stillthread = CustomToolItem.CreateTool<Stillthread>(plugin, stillthreadData);
            CustomToolManager.RegisterTool(stillthread);
        }
    }
}
