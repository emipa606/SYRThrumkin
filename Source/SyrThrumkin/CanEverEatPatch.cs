using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch(typeof(RaceProperties), "CanEverEat", typeof(ThingDef))]
    public static class CanEverEatPatch
    {
        // Token: 0x0600000E RID: 14 RVA: 0x00002401 File Offset: 0x00000601
        [HarmonyPrefix]
        public static bool CanEverEat_Prefix(ref bool __result, RaceProperties __instance, ThingDef t)
        {
            if (__instance.body != ThrumkinDefOf.Thrumkin_Body || t != ThingDefOf.WoodLog && t != ThingDefOf.Hay)
            {
                return true;
            }

            __result = true;
            return false;
        }
    }
}