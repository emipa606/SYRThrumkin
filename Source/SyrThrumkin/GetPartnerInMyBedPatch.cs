using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000013 RID: 19
    [HarmonyPatch(typeof(LovePartnerRelationUtility), "GetPartnerInMyBed")]
    public class GetPartnerInMyBedPatch
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002A20 File Offset: 0x00000C20
        [HarmonyPostfix]
        public static void GetPartnerInMyBed_Postfix(ref Pawn __result, Pawn pawn)
        {
            var building_Bed = pawn.CurrentBed();
            if (pawn == null || building_Bed == null)
            {
                return;
            }

            foreach (var pawn2 in building_Bed.CurOccupants)
            {
                if (pawn2 != pawn && (pawn.def == ThrumkinDefOf.Thrumkin || pawn2.def == ThrumkinDefOf.Thrumkin) &&
                    pawn.ageTracker.AgeBiologicalYearsFloat >= 16f &&
                    pawn2.ageTracker.AgeBiologicalYearsFloat >= 16f)
                {
                    __result = pawn2;
                }
            }
        }
    }
}