using HarmonyLib;
using RimWorld.SketchGen;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(SketchGenUtility), "IsFloorAllowed")]
    public static class IsFloorAllowedPatch
    {
        // Token: 0x0600000B RID: 11 RVA: 0x0000233E File Offset: 0x0000053E
        [HarmonyPostfix]
        public static void IsFloorAllowed_Postfix(ref bool __result, TerrainDef floor, bool allowWoodenFloor)
        {
            if (!allowWoodenFloor && floor == ThrumkinDefOf.GhostAshFloor)
            {
                __result = false;
            }
        }
    }
}