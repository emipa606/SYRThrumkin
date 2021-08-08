using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000011 RID: 17
    [HarmonyPatch(typeof(ThoughtWorker_SharedBed), "CurrentStateInternal")]
    public class ThoughtWorker_SharedBedPatch
    {
        // Token: 0x06000019 RID: 25 RVA: 0x000029B4 File Offset: 0x00000BB4
        [HarmonyPostfix]
        public static void ThoughtWorker_SharedBed_Postfix(ref ThoughtState __result, Pawn p)
        {
            if (p == null)
            {
                return;
            }

            var mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p);
            if (mostDislikedNonPartnerBedOwner?.def == null)
            {
                return;
            }

            if (p.def == ThrumkinDefOf.Thrumkin)
            {
                __result = false;
            }

            if (LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p).def == ThrumkinDefOf.Thrumkin)
            {
                __result = false;
            }
        }
    }
}