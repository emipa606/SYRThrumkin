using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(PlantUtility), "GrowthSeasonNow")]
    public static class GrowthSeasonNowPatch
    {
        // Token: 0x06000009 RID: 9 RVA: 0x000022FD File Offset: 0x000004FD
        [HarmonyPostfix]
        public static void GrowthSeasonNow_Postfix(ref bool __result, IntVec3 c, Map map, bool forSowing = false)
        {
            var plant = c.GetPlant(map);
            if (plant?.def == null)
            {
                return;
            }

            var plant2 = c.GetPlant(map);
            if (plant2?.def == ThrumkinDefOf.Plant_Frostleaf)
            {
                __result = true;
            }
        }
    }
}