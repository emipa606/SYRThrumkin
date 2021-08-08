using System.Collections.Generic;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200001D RID: 29
    internal class SyrThrumkinCore : Mod
    {
        // Token: 0x04000022 RID: 34
        public static SyrThrumkinSettings settings;

        // Token: 0x06000031 RID: 49 RVA: 0x000032F8 File Offset: 0x000014F8
        public SyrThrumkinCore(ModContentPack content) : base(content)
        {
            settings = GetSettings<SyrThrumkinSettings>();
            new Harmony("Syrchalis.Rimworld.Thrumkin").PatchAll(Assembly.GetExecutingAssembly());
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00003320 File Offset: 0x00001520
        public override string SettingsCategory()
        {
            return "SyrThrumkinSettingsCategory".Translate();
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003334 File Offset: 0x00001534
        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("SyrThrumkin_useUnsupportedHair".Translate(),
                ref SyrThrumkinSettings.useUnsupportedHair, "SyrThrumkin_useUnsupportedHairTooltip".Translate());
            listing_Standard.CheckboxLabeled("SyrThrumkin_useStandardAI".Translate(),
                ref SyrThrumkinSettings.useStandardAI, "SyrThrumkin_useStandardAITooltip".Translate());
            listing_Standard.CheckboxLabeled("SyrThrumkin_manualWoodConsumption".Translate(),
                ref SyrThrumkinSettings.manualWoodConsumption, "SyrThrumkin_manualWoodConsumptionTooltip".Translate());
            listing_Standard.Gap(24f);
            if (listing_Standard.ButtonText("SyrThrumkin_defaultSettings".Translate(),
                "SyrThrumkin_defaultSettingsTooltip".Translate()))
            {
                SyrThrumkinSettings.useUnsupportedHair = false;
                SyrThrumkinSettings.useStandardAI = false;
            }

            listing_Standard.End();
            settings.Write();
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003414 File Offset: 0x00001614
        public override void WriteSettings()
        {
            base.WriteSettings();
            ApplySettings();
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003424 File Offset: 0x00001624
        public static void ApplySettings()
        {
            if (SyrThrumkinSettings.useUnsupportedHair)
            {
                var thingDef_AlienRace = ThrumkinDefOf.Thrumkin as ThingDef_AlienRace;
                thingDef_AlienRace?.alienRace.styleSettings[typeof(HairDef)].styleTagsOverride
                    .AddRange(new List<string> {"Tribal", "Rural", "Urban", "Punk"});
                return;
            }

            if (ThrumkinDefOf.Thrumkin is ThingDef_AlienRace thingDef_AlienRace2)
            {
                thingDef_AlienRace2.alienRace.styleSettings[typeof(HairDef)].styleTagsOverride =
                    new List<string> {"Thrumkin"};
            }
        }
    }
}