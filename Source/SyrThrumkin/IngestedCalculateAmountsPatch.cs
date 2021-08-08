using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(Thing), "IngestedCalculateAmounts")]
    public class IngestedCalculateAmountsPatch
    {
        // Token: 0x06000011 RID: 17 RVA: 0x00002454 File Offset: 0x00000654
        [HarmonyPostfix]
        public static void IngestedCalculateAmounts_Postfix(Thing __instance, Pawn ingester, float nutritionWanted,
            ref float nutritionIngested)
        {
            if (ingester?.def == null)
            {
                return;
            }

            bool doEat;
            if (__instance == null)
            {
                doEat = false;
            }
            else
            {
                var def = __instance.def;
                doEat = def?.ingestible != null;
            }

            if (!doEat || ingester.def != ThrumkinDefOf.Thrumkin)
            {
                return;
            }

            if (__instance.def == ThingDefOf.MealSurvivalPack || __instance.def == ThingDefOf.Pemmican ||
                __instance.def == ThrumkinDefOf.MealLavish)
            {
                nutritionIngested *= 1.2f;
                return;
            }

            var compIngredients = __instance.TryGetComp<CompIngredients>();
            if (compIngredients?.ingredients != null)
            {
                if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible1 = i1.ingestible;
                    return ((ingestible1?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i4)
                {
                    var ingestible4 = i4.ingestible;
                    return ((ingestible4?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }))
                {
                    return;
                }

                if (compIngredients.ingredients.Exists(delegate(ThingDef i5)
                {
                    var ingestible5 = i5.ingestible;
                    return ((ingestible5?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible = i.ingestible;
                    return ((ingestible?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) == FoodTypeFlags.None;
                }))
                {
                    nutritionIngested *= 2f;
                    return;
                }

                if (compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible2 = i2.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i7)
                {
                    var ingestible7 = i7.ingestible;
                    return ((ingestible7?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    nutritionIngested *= 0.35f;
                    return;
                }

                if (compIngredients.ingredients.Exists(delegate(ThingDef i6)
                {
                    var ingestible6 = i6.ingestible;
                    return ((ingestible6?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i9)
                {
                    var ingestible9 = i9.ingestible;
                    return ((ingestible9?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) == FoodTypeFlags.None;
                }))
                {
                    nutritionIngested *= 1.5f;
                    return;
                }

                if (compIngredients.ingredients.Exists(delegate(ThingDef i3)
                {
                    var ingestible3 = i3.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i8)
                {
                    var ingestible8 = i8.ingestible;
                    return ((ingestible8?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    nutritionIngested *= 0.7f;
                }
            }
            else
            {
                if ((__instance.def.ingestible.foodType & FoodTypeFlags.Meat) != FoodTypeFlags.None)
                {
                    nutritionIngested *= 0.35f;
                    return;
                }

                if ((__instance.def.ingestible.foodType & FoodTypeFlags.AnimalProduct) != FoodTypeFlags.None)
                {
                    nutritionIngested *= 2f;
                }
            }
        }
    }
}