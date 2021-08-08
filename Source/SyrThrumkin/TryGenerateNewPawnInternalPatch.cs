using HarmonyLib;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000017 RID: 23
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class TryGenerateNewPawnInternalPatch
    {
        // Token: 0x06000023 RID: 35 RVA: 0x00002CF3 File Offset: 0x00000EF3
        [HarmonyPriority(0)]
        [HarmonyPostfix]
        public static void TryGenerateNewPawnInternal_Postfix(ref Pawn __result, ref PawnGenerationRequest request)
        {
            __result = TryGenerateNewPawnInternal_Method(__result, request);
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002D04 File Offset: 0x00000F04
        public static Pawn TryGenerateNewPawnInternal_Method(Pawn __result, PawnGenerationRequest request)
        {
            bool hasChildhood;
            if (__result == null)
            {
                hasChildhood = false;
            }
            else
            {
                var story = __result.story;
                hasChildhood = story?.childhood != null;
            }

            if (!hasChildhood)
            {
                return __result;
            }

            var story2 = __result.story;

            if (story2?.adulthood != null && !TryGenerateNewLeaderPatch.PrefixRunning &&
                (__result.story.childhood == ThrumkinDefOf.Thrumkin_CBio_Menardy.backstory ||
                 __result.story.adulthood == ThrumkinDefOf.Thrumkin_ABio_Menardy.backstory))
            {
                return null;
            }

            return __result;
        }
    }
}