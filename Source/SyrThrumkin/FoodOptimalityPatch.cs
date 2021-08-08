using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200000F RID: 15
    [HarmonyPatch(typeof(FoodUtility), "FoodOptimality")]
    public static class FoodOptimalityPatch
    {
        // Token: 0x06000015 RID: 21 RVA: 0x000026B4 File Offset: 0x000008B4
        [HarmonyPriority(800)]
        [HarmonyPostfix]
        public static void FoodOptimality_Postfix(ref float __result, Pawn eater, Thing foodSource)
        {
            if (SyrThrumkinSettings.useStandardAI)
            {
                return;
            }

            __result = FoodOptimality_Method(__result, eater, foodSource);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000026CC File Offset: 0x000008CC
        public static float FoodOptimality_Method(float __result, Pawn eater, Thing foodSource)
        {
            var num = __result;
            if (eater?.def == null)
            {
                return num;
            }

            bool doEat;
            if (foodSource == null)
            {
                doEat = false;
            }
            else
            {
                var def = foodSource.def;
                doEat = def?.ingestible != null;
            }

            if (!doEat || eater.def != ThrumkinDefOf.Thrumkin)
            {
                return num;
            }

            if (foodSource.def == ThingDefOf.MealSurvivalPack || foodSource.def == ThingDefOf.Pemmican ||
                foodSource.def == ThrumkinDefOf.MealLavish)
            {
                return num;
            }

            if (foodSource.def == ThingDefOf.WoodLog || foodSource.def == ThingDefOf.Hay)
            {
                num -= 50f;
            }

            var compIngredients = foodSource.TryGetComp<CompIngredients>();
            if (compIngredients?.ingredients != null)
            {
                if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible1 = i1.ingestible;
                    return ((ingestible1?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible2 = i2.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }))
                {
                    num += 0f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible1 = i1.ingestible;
                    return ((ingestible1?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i3)
                {
                    var ingestible3 = i3.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) == FoodTypeFlags.None;
                }))
                {
                    num += 40f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible = i.ingestible;
                    return ((ingestible?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible2 = i2.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    num -= 50f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible1 = i1.ingestible;
                    return ((ingestible1?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible2 = i2.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) == FoodTypeFlags.None;
                }))
                {
                    num += 20f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible = i.ingestible;
                    return ((ingestible?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible1 = i1.ingestible;
                    return ((ingestible1?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    num -= 10f;
                }
            }
            else if ((foodSource.def.ingestible.foodType & FoodTypeFlags.Meat) != FoodTypeFlags.None)
            {
                num -= 70f;
            }
            else if ((foodSource.def.ingestible.foodType & FoodTypeFlags.AnimalProduct) != FoodTypeFlags.None)
            {
                num += 30f;
            }

            return num;
        }
    }
}