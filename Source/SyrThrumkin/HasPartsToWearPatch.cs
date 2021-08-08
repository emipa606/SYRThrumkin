using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear")]
    public static class HasPartsToWearPatch
    {
        // Token: 0x0600000C RID: 12 RVA: 0x00002350 File Offset: 0x00000550
        [HarmonyPostfix]
        public static void HasPartsToWear_Postfix(ref bool __result, Pawn p, ThingDef apparel)
        {
            bool hasApparel;
            if (apparel == null)
            {
                hasApparel = false;
            }
            else
            {
                var apparel2 = apparel.apparel;
                hasApparel = apparel2?.bodyPartGroups != null;
            }

            if (hasApparel && p?.def != null && p.def == ThrumkinDefOf.Thrumkin &&
                apparel.apparel.bodyPartGroups.Contains(ThrumkinDefOf.Feet) &&
                (!apparel.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) ||
                 !apparel.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso)))
            {
                __result = false;
            }
        }
    }
}