using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000005 RID: 5
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        // Token: 0x06000008 RID: 8 RVA: 0x000022F6 File Offset: 0x000004F6
        static HarmonyPatches()
        {
            SyrThrumkinCore.ApplySettings();
        }
    }
}