using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000E RID: 14
    [HarmonyPatch(typeof(Thing), "Ingested")]
    public class IngestedPatch
    {
        // Token: 0x06000013 RID: 19 RVA: 0x00002620 File Offset: 0x00000820
        [HarmonyPostfix]
        public static void Ingested_Postfix(Thing __instance, Pawn ingester, float nutritionWanted)
        {
            if (ingester?.def == null)
            {
                return;
            }

            var compIngredients = __instance.TryGetComp<CompIngredients>();
            if (compIngredients?.ingredients != null &&
                compIngredients.ingredients.Contains(ThrumkinDefOf.RawFrostleaves))
            {
                ingester.health.AddHediff(ThrumkinDefOf.AteFrostLeaves);
            }

            if (__instance.def == ThingDefOf.WoodLog && ingester.def == ThrumkinDefOf.Thrumkin)
            {
                ingester.health.AddHediff(ThrumkinDefOf.AteWoodHediff);
            }
        }
    }
}