using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200001C RID: 28
    internal class Recipe_RemoveHornThrumkin : Recipe_Surgery
    {
        // Token: 0x0600002C RID: 44 RVA: 0x00003069 File Offset: 0x00001269
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            var notMissingParts = pawn.health.hediffSet.GetNotMissingParts();
            foreach (var bodyPartRecord in notMissingParts)
            {
                if (bodyPartRecord.def == ThrumkinDefOf.HornThrumkin)
                {
                    yield return bodyPartRecord;
                }
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x0000307C File Offset: 0x0000127C
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients,
            Bill bill)
        {
            if (billDoer != null)
            {
                if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
                {
                    return;
                }

                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                GenSpawn.Spawn(part.def.spawnThingOnRemoved, billDoer.Position, billDoer.Map);
            }

            var surgicalCutSpecial = ThrumkinDefOf.SurgicalCutSpecial;
            var num = 99999f;
            var num2 = 999f;
            pawn.TakeDamage(new DamageInfo(surgicalCutSpecial, num, num2, -1f, null, part));
            if (!pawn.health.hediffSet.hediffs.Any(d =>
                !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
            {
                if (pawn.Dead)
                {
                    ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
                }

                GiveThoughtsForPawnHornHarvested(pawn);
            }

            if (!IsViolationOnPawn(pawn, part, Faction.OfPlayer) || pawn.Faction == null || billDoer == null ||
                billDoer.Faction == null)
            {
                return;
            }

            var faction = pawn.Faction;
            var faction2 = billDoer.Faction;
            var num3 = -15;
            string text = "GoodwillChangedReason_RemovedBodyPart".Translate(part.LabelShort);
            faction.TryAffectGoodwillWith(faction2, num3, true, true, null, pawn);
            faction.Notify_GoodwillSituationsChanged(faction2, true, text, pawn);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000031D4 File Offset: 0x000013D4
        public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
        {
            if (pawn.health.hediffSet.hediffs.Any(d =>
                !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
            {
                return recipe.label;
            }

            return "SyrThrumkinStealHorn".Translate();
        }

        // Token: 0x0600002F RID: 47 RVA: 0x0000322C File Offset: 0x0000142C
        public static void GiveThoughtsForPawnHornHarvested(Pawn victim)
        {
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }

            ThoughtDef thoughtDef = null;
            if (victim.IsColonist)
            {
                thoughtDef = ThrumkinDefOf.KnowColonistHornHarvested;
            }
            else if (victim.HostFaction == Faction.OfPlayer)
            {
                thoughtDef = ThrumkinDefOf.KnowOtherHornHarvested;
            }

            foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (pawn == victim)
                {
                    pawn.needs.mood.thoughts.memories.TryGainMemory(ThrumkinDefOf.MyHornHarvested);
                }
                else if (thoughtDef != null)
                {
                    pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef);
                }
            }
        }
    }
}