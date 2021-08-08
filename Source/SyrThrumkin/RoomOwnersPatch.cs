using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000012 RID: 18
    [HarmonyPatch(typeof(Room), "Owners", MethodType.Getter)]
    public class RoomOwnersPatch
    {
        // Token: 0x0600001B RID: 27 RVA: 0x00002A0F File Offset: 0x00000C0F
        [HarmonyPostfix]
        public static IEnumerable<Pawn> RoomOwners_Postfix(IEnumerable<Pawn> __result, Room __instance)
        {
            if (__instance.TouchesMapEdge)
            {
                yield break;
            }

            if (__instance.IsHuge)
            {
                yield break;
            }

            if (__instance.Role != RoomRoleDefOf.Bedroom && __instance.Role != RoomRoleDefOf.PrisonCell &&
                __instance.Role != RoomRoleDefOf.Barracks && __instance.Role != RoomRoleDefOf.PrisonBarracks)
            {
                yield break;
            }

            Pawn pawn = null;
            Pawn secondOwner = null;
            foreach (var building_Bed in __instance.ContainedBeds)
            {
                if (!building_Bed.def.building.bed_humanlike)
                {
                    continue;
                }

                foreach (var owner in building_Bed.OwnersForReading)
                {
                    if (pawn == null)
                    {
                        pawn = owner;
                    }
                    else
                    {
                        if (secondOwner != null)
                        {
                            yield break;
                        }

                        secondOwner = owner;
                    }
                }
            }

            if (pawn == null)
            {
                yield break;
            }

            yield return pawn;
            if (secondOwner != null)
            {
                yield return secondOwner;
            }
        }
    }
}