using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace SyrThrumkin
{
    // Token: 0x0200001B RID: 27
    public class JobDriver_ShearThrumkin : JobDriver_GatherAnimalBodyResources
    {
        // Token: 0x04000021 RID: 33
        private float gatherProgress;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000028 RID: 40 RVA: 0x00003042 File Offset: 0x00001242
        protected override float WorkTotal => 850f;

        // Token: 0x06000029 RID: 41 RVA: 0x00003049 File Offset: 0x00001249
        protected override CompHasGatherableBodyResource GetComp(Pawn pawn)
        {
            return pawn.TryGetComp<CompShearable>();
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00003051 File Offset: 0x00001251
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            var wait = new Toil();
            wait.initAction = delegate
            {
                var actor = wait.actor;
                var thing = (Pawn) job.GetTarget(TargetIndex.A).Thing;
                actor.pather.StopDead();
                PawnUtility.ForceWait(thing, 15000, null, true);
            };
            wait.tickAction = delegate
            {
                var actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Animals, 0.13f);
                gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed);
                if (!(gatherProgress >= WorkTotal))
                {
                    return;
                }

                GetComp((Pawn) (Thing) job.GetTarget(TargetIndex.A)).Gathered(pawn);
                actor.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            wait.AddFinishAction(delegate
            {
                var thing = (Pawn) job.GetTarget(TargetIndex.A).Thing;
                if (thing != null && thing.CurJobDef == JobDefOf.Wait_MaintainPosture)
                {
                    thing.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            });
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(delegate
            {
                if (!GetComp((Pawn) (Thing) job.GetTarget(TargetIndex.A)).ActiveAndFull)
                {
                    return JobCondition.Incompletable;
                }

                return JobCondition.Ongoing;
            });
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.WithProgressBar(TargetIndex.A, () => gatherProgress / WorkTotal);
            wait.activeSkill = () => SkillDefOf.Animals;
            yield return wait;
        }
    }
}