using Verse;

namespace SyrThrumkin
{
    // Token: 0x0200001E RID: 30
    internal class SyrThrumkinSettings : ModSettings
    {
        // Token: 0x04000023 RID: 35
        public static bool useUnsupportedHair;

        // Token: 0x04000024 RID: 36
        public static bool useStandardAI;

        // Token: 0x04000025 RID: 37
        public static bool manualWoodConsumption;

        // Token: 0x06000036 RID: 54 RVA: 0x000034E2 File Offset: 0x000016E2
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref useUnsupportedHair, "SyrThrumkin_useUnsupportedHair", false, true);
            Scribe_Values.Look(ref useStandardAI, "SyrThrumkin_usestandardAI", false, true);
            Scribe_Values.Look(ref manualWoodConsumption, "SyrThrumkin_manualWoodConsumption", false, true);
        }
    }
}