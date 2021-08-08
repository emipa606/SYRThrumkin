using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch(typeof(Need_Food), "MaxLevel", MethodType.Getter)]
    public static class Need_FoodPatch
    {
        // Token: 0x0600000D RID: 13 RVA: 0x000023DA File Offset: 0x000005DA
        [HarmonyPostfix]
        public static void Need_Food_Postfix(ref float __result, Pawn ___pawn)
        {
            if (___pawn?.def != null && ___pawn.def == ThrumkinDefOf.Thrumkin)
            {
                __result *= 1.5f;
            }
        }
    }
}