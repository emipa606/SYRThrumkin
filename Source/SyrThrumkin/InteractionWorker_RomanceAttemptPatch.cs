using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000015 RID: 21
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public static class InteractionWorker_RomanceAttemptPatch
    {
        // Token: 0x06000021 RID: 33 RVA: 0x00002B24 File Offset: 0x00000D24
        [HarmonyPostfix]
        [HarmonyPriority(0)]
        public static void RandomSelectionWeight_Postfix(ref float __result, Pawn initiator, Pawn recipient)
        {
            if (initiator?.def == null ||
                recipient?.def == null)
            {
                return;
            }

            if (initiator.def == ThrumkinDefOf.Thrumkin)
            {
                __result *= 0.5f;
                return;
            }

            if (recipient.def == ThrumkinDefOf.Thrumkin)
            {
                __result *= 0.5f;
            }
        }
    }
}