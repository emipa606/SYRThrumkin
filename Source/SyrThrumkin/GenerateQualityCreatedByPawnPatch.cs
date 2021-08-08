using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000C RID: 12
    [HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn", typeof(Pawn), typeof(SkillDef))]
    public class GenerateQualityCreatedByPawnPatch
    {
        // Token: 0x0600000F RID: 15 RVA: 0x00002426 File Offset: 0x00000626
        [HarmonyPostfix]
        public static void GenerateQualityCreatedByPawn_Postfix(ref QualityCategory __result, Pawn pawn,
            SkillDef relevantSkill)
        {
            if (pawn.def == ThrumkinDefOf.Thrumkin && __result != QualityCategory.Awful && Rand.Value < 0.2f)
            {
                __result = (QualityCategory) (__result - QualityCategory.Poor);
            }
        }
    }
}