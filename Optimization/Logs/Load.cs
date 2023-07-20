using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class Load
    {
        public static void Init()
        {
            IL.RoR2.RunReport.Load += RunReport_Load;
        }

        private static void RunReport_Load(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchBrtrue(out _),
                x => x.MatchLdstr("Could not load RunReport {0}: {1}")))
            {
                c.Index++;
                for (int i = 0; i < 12; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Run Report Load 1 hook");
            }

            c.Index = 0;

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(out _),
                x => x.MatchLdstr("Could not load RunReport {0}: {1}")))
            {
                c.Index++;
                for (int i = 0; i < 13; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Run Report Load 2 hook");
            }
        }
    }
}