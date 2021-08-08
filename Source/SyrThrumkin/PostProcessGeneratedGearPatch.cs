using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000018 RID: 24
    [HarmonyPatch(typeof(PawnGenerator), "PostProcessGeneratedGear")]
    public static class PostProcessGeneratedGearPatch
    {
        // Token: 0x06000025 RID: 37 RVA: 0x00002D80 File Offset: 0x00000F80
        [HarmonyPostfix]
        public static void PostProcessGeneratedGear_Postfix(Thing gear, Pawn pawn)
        {
            bool hasChildhood;
            if (pawn == null)
            {
                hasChildhood = false;
            }
            else
            {
                var story = pawn.story;
                hasChildhood = story?.childhood != null;
            }

            if (!hasChildhood)
            {
                return;
            }

            var story2 = pawn.story;

            if (story2?.adulthood == null || gear.def == null ||
                gear.def != ThrumkinDefOf.Apparel_PlateArmor && gear.def != ThrumkinDefOf.Apparel_TribalA &&
                !gear.def.IsMeleeWeapon ||
                pawn.story.childhood != ThrumkinDefOf.Thrumkin_CBio_Menardy.backstory &&
                pawn.story.adulthood != ThrumkinDefOf.Thrumkin_ABio_Menardy.backstory ||
                pawn.Faction == Faction.OfPlayerSilentFail)
            {
                return;
            }

            if (gear.def == ThrumkinDefOf.Apparel_TribalA)
            {
                gear.SetStuffDirect(ThrumkinDefOf.DevilstrandCloth);
                gear.HitPoints = 130;
                gear.SetColor(new Color(0.7f, 0.23f, 0.23f));
                return;
            }

            gear.SetStuffDirect(ThingDefOf.Plasteel);
            gear.HitPoints = gear.def.IsMeleeWeapon ? 280 : 810;
        }
    }
}