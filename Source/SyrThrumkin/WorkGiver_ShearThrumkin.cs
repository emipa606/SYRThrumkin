using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace SyrThrumkin
{
    // Token: 0x0200001F RID: 31
    public class WorkGiver_ShearThrumkin : WorkGiver_GatherAnimalBodyResources
    {
        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000038 RID: 56 RVA: 0x00003525 File Offset: 0x00001725
        protected override JobDef JobDef => ThrumkinDefOf.ShearThrumkin;

        // Token: 0x06000039 RID: 57 RVA: 0x00003049 File Offset: 0x00001249
        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return animal.TryGetComp<CompShearable>();
        }

        // Token: 0x0600003A RID: 58 RVA: 0x0000352C File Offset: 0x0000172C
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            var pawns = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
            int num;
            for (var i = 0; i < pawns.Count; i = num + 1)
            {
                if (pawns[i] != pawn)
                {
                    yield return pawns[i];
                }

                num = i;
            }
        }

        // Token: 0x0600003B RID: 59 RVA: 0x0000353C File Offset: 0x0000173C
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            var freeColonistsAndPrisonersSpawned = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
            foreach (var animal in freeColonistsAndPrisonersSpawned)
            {
                if (animal.def != ThrumkinDefOf.Thrumkin)
                {
                    continue;
                }

                var comp = GetComp(animal);
                if (comp is {ActiveAndFull: true})
                {
                    return false;
                }
            }

            return true;
        }

        // Token: 0x0600003C RID: 60 RVA: 0x0000359C File Offset: 0x0000179C
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Pawn pawn2) || pawn2.def != ThrumkinDefOf.Thrumkin)
            {
                return false;
            }

            var comp = GetComp(pawn2);
            return comp is {ActiveAndFull: true} && !pawn2.Downed && pawn2.CanCasuallyInteractNow() &&
                pawn.CanReserve(pawn2, 1, -1, null, forced) || comp.ActiveAndFull &&
                pawn2.CanCasuallyInteractNow() && pawn.CanReserve(pawn2, 1, -1, null, forced) &&
                pawn2.IsPrisonerOfColony && pawn2.guest.PrisonerIsSecure && pawn2.Spawned &&
                !pawn2.InAggroMentalState && !t.IsForbidden(pawn) && !pawn2.IsFormingCaravan() &&
                pawn.CanReserveAndReach(pawn2, PathEndMode.OnCell, pawn.NormalMaxDanger());
        }
    }
}