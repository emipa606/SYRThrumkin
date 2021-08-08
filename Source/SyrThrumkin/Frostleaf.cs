using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000003 RID: 3
    public class Frostleaf : Plant
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002052 File Offset: 0x00000252
        public override float GrowthRate
        {
            get
            {
                if (Blighted)
                {
                    return 0f;
                }

                return GrowthRateFactor_Fertility * GrowthRateFactor_TemperatureFrostleaf * GrowthRateFactor_Light;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
        public float GrowthRateFactor_TemperatureFrostleaf
        {
            get
            {
                if (!GenTemperature.TryGetTemperatureForCell(Position, Map, out var num))
                {
                    return 1f;
                }

                if (num < -10f)
                {
                    return Mathf.InverseLerp(-50f, -10f, num);
                }

                if (num > 42f)
                {
                    return Mathf.InverseLerp(58f, 42f, num);
                }

                return 1f;
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020D8 File Offset: 0x000002D8
        public override string GetInspectString()
        {
            var stringBuilder = new StringBuilder();
            switch (LifeStage)
            {
                case PlantLifeStage.Growing:
                {
                    stringBuilder.AppendLine("PercentGrowth".Translate(GrowthPercentString));
                    stringBuilder.AppendLine("GrowthRate".Translate() + ": " + GrowthRate.ToStringPercent());
                    if (!Blighted)
                    {
                        if (Resting)
                        {
                            stringBuilder.AppendLine("PlantResting".Translate());
                        }

                        if (!HasEnoughLightToGrow)
                        {
                            stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " +
                                                     def.plant.growMinGlow.ToStringPercent());
                        }

                        var growthRateFactor_TemperatureFrostleaf = GrowthRateFactor_TemperatureFrostleaf;
                        if (growthRateFactor_TemperatureFrostleaf < 0.99f)
                        {
                            if (growthRateFactor_TemperatureFrostleaf < 0.01f)
                            {
                                stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
                            }
                            else
                            {
                                stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(
                                    Mathf.RoundToInt(growthRateFactor_TemperatureFrostleaf * 100f).ToString()));
                            }
                        }
                    }

                    break;
                }
                case PlantLifeStage.Mature:
                    stringBuilder.AppendLine(HarvestableNow ? "ReadyToHarvest".Translate() : "Mature".Translate());
                    break;
            }

            if (DyingBecauseExposedToLight)
            {
                stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
            }

            if (Blighted)
            {
                stringBuilder.AppendLine("Blighted".Translate() + " (" + Blight.Severity.ToStringPercent() + ")");
            }

            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}