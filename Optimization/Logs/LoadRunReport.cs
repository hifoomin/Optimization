using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class LoadRunReport
    {
        public static void Init()
        {
            IL.RoR2.MorgueManager.LoadHistoryRunReports += MorgueManager_LoadHistoryRunReports;
        }

        private static void MorgueManager_LoadHistoryRunReports(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Could not load RunReport \"{0}\": {1}")))
            {
                for (int i = 0; i < 14; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Morgue Manager Load History Run Reports hook");
            }
        }
    }
}