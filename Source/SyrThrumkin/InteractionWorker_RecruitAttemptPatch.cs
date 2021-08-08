using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000019 RID: 25
    [HarmonyPatch(typeof(InteractionWorker_RecruitAttempt), "Interacted")]
    public static class InteractionWorker_RecruitAttemptPatch
    {
        // Token: 0x06000026 RID: 38 RVA: 0x00002EA0 File Offset: 0x000010A0
        [HarmonyPostfix]
        public static void InteractionWorker_RecruitAttempt_Postfix(Pawn initiator, Pawn recipient)
        {
            if (initiator?.def == null ||
                recipient?.def == null || initiator.def != ThrumkinDefOf.Thrumkin ||
                recipient.def != ThingDefOf.Thrumbo || recipient.Faction != null || !Rand.Chance(0.25f))
            {
                return;
            }

            InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, false);
            var value = recipient.KindLabelIndefinite();
            if (recipient.Name != null)
            {
                Messages.Message(
                    "ThrumkinThrumboTameNameSuccess"
                        .Translate(initiator.LabelShort, value, recipient.Name.ToStringFull).AdjustedFor(recipient),
                    recipient, MessageTypeDefOf.PositiveEvent);
            }
            else
            {
                Messages.Message("ThrumkinThrumboTameSuccess".Translate(initiator.LabelShort, value), recipient,
                    MessageTypeDefOf.PositiveEvent);
            }

            if (initiator.Spawned && recipient.Spawned)
            {
                MoteMaker.ThrowText(((initiator.DrawPos + recipient.DrawPos) / 2f) + new Vector3(0f, 0f, 1f),
                    initiator.Map, "TextMote_TameSuccess".Translate("25%"), 8f);
            }
        }
    }
}