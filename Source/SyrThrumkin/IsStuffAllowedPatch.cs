using HarmonyLib;
using RimWorld.SketchGen;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(SketchGenUtility), "IsStuffAllowed")]
    public static class IsStuffAllowedPatch
    {
        // Token: 0x0600000A RID: 10 RVA: 0x00002331 File Offset: 0x00000531
        [HarmonyPostfix]
        public static void IsStuffAllowed_Postfix(ref bool __result, ThingDef stuff, bool allowWood,
            Map useOnlyStonesAvailableOnMap, bool allowFlammableWalls, ThingDef stuffFor)
        {
            if (stuff == ThrumkinDefOf.WoodLog_GhostAsh)
            {
                __result = false;
            }
        }
    }
}