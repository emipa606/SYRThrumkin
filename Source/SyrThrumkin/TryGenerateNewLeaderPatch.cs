using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000016 RID: 22
    [HarmonyPatch(typeof(Faction), "TryGenerateNewLeader")]
    public static class TryGenerateNewLeaderPatch
    {
        // Token: 0x04000020 RID: 32
        public static bool PrefixRunning;

        // Token: 0x06000022 RID: 34 RVA: 0x00002B7C File Offset: 0x00000D7C
        [HarmonyPrefix]
        public static bool TryGenerateNewLeader_Prefix(Faction __instance)
        {
            if (!Current.Game.GetComponent<GameComponent_MenardySpawnable>().MenardySpawnable ||
                __instance?.def == null || __instance.def != ThrumkinDefOf.ThrumkinTribe ||
                !(Rand.Value > 0.5f))
            {
                return true;
            }

            __instance.leader = null;
            PrefixRunning = true;
            var request = new PawnGenerationRequest(ThrumkinDefOf.Thrumkin_ElderMelee, __instance,
                PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f, false, true, true,
                false, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null,
                Gender.Female);
            var num = 0;
            Pawn pawn;
            do
            {
                pawn = PawnGenerator.GeneratePawn(request);
                num++;
            } while ((pawn.story.childhood != ThrumkinDefOf.Thrumkin_CBio_Menardy.backstory ||
                      pawn.story.adulthood != ThrumkinDefOf.Thrumkin_ABio_Menardy.backstory) && num < 1000);

            if (num >= 1000)
            {
                Log.Warning("Thrumkin - Generated too many pawns when attempting to get specific backstory");
            }

            pawn.Name = ThrumkinDefOf.Thrumkin_Bio_Menardy.name;
            pawn.story.hairDef = ThrumkinDefOf.Thrumkin_Hair_3;
            __instance.leader = pawn;
            pawn.relations.everSeenByPlayer = true;
            if (!Find.WorldPawns.Contains(pawn))
            {
                Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
            }

            Current.Game.GetComponent<GameComponent_MenardySpawnable>().MenardySpawnable = false;
            PrefixRunning = false;
            return false;
        }
    }
}