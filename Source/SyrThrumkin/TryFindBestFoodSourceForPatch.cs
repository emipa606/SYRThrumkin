using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SyrThrumkin
{
    // Token: 0x02000010 RID: 16
    [HarmonyPatch(typeof(FoodUtility), "TryFindBestFoodSourceFor")]
    public class TryFindBestFoodSourceForPatch
    {
        // Token: 0x06000017 RID: 23 RVA: 0x000028B8 File Offset: 0x00000AB8
        [HarmonyPostfix]
        public static void TryFindBestFoodSourceFor_Postfix(ref bool __result, Pawn getter, Pawn eater, bool desperate,
            ref Thing foodSource, ref ThingDef foodDef, bool allowForbidden, bool allowSociallyImproper,
            bool ignoreReservations)
        {
            if (SyrThrumkinSettings.manualWoodConsumption || foodSource != null || foodDef != null)
            {
                return;
            }

            if (eater?.def == null || eater.def != ThrumkinDefOf.Thrumkin ||
                eater.needs.food.CurCategory < HungerCategory.UrgentlyHungry)
            {
                return;
            }

            var thingReq = ThingRequest.ForGroup(ThingRequestGroup.FoodSource);

            bool Validator(Thing t)
            {
                return (t.def == ThingDefOf.WoodLog || t.def == ThingDefOf.Hay) &&
                       !t.IsForbidden(eater) | allowForbidden && eater.WillEat(t, getter) && t.IngestibleNow &&
                       (t.IsSociallyProper(getter) || t.IsSociallyProper(eater, eater.IsPrisonerOfColony)) |
                       allowSociallyImproper && getter.CanReserve(t, 10) | ignoreReservations;
            }

            var thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingReq,
                PathEndMode.Touch, TraverseParms.For(getter), 75f, Validator, null, 0, -1, false,
                RegionType.Normal | RegionType.Portal);
            if (thing == null)
            {
                __result = false;
                return;
            }

            foodSource = thing;
            foodDef = FoodUtility.GetFinalIngestibleDef(thing);
            __result = true;
        }
    }
}