using Verse;

namespace SyrThrumkin
{
    // Token: 0x02000004 RID: 4
    public class GameComponent_MenardySpawnable : GameComponent
    {
        // Token: 0x0400001F RID: 31
        public bool MenardySpawnable = true;

        // Token: 0x06000006 RID: 6 RVA: 0x000022CD File Offset: 0x000004CD
        public GameComponent_MenardySpawnable(Game game)
        {
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000022DC File Offset: 0x000004DC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref MenardySpawnable, "Thrumkin_MenardySpawnable", true);
        }
    }
}