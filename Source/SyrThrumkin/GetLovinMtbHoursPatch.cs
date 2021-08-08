using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000014 RID: 20
    [HarmonyPatch(typeof(LovePartnerRelationUtility), "GetLovinMtbHours")]
    public class GetLovinMtbHoursPatch
    {
        // Token: 0x0600001F RID: 31 RVA: 0x00002AB8 File Offset: 0x00000CB8
        [HarmonyPostfix]
        public static void GetLovinMtbHours_Postfix(ref float __result, Pawn pawn, Pawn partner)
        {
            if (pawn?.def == null || partner?.def == null)
            {
                return;
            }

            if (pawn.def == ThrumkinDefOf.Thrumkin && partner.def == ThrumkinDefOf.Thrumkin)
            {
                __result = 12f;
                return;
            }

            if (pawn.def == ThrumkinDefOf.Thrumkin || partner.def == ThrumkinDefOf.Thrumkin)
            {
                __result = 24f;
            }
        }
    }
}