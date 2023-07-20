using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class Save
    {
        public static void Init()
        {
            IL.RoR2.RunReport.Save += RunReport_Save;
        }

        private static void RunReport_Save(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Could not save RunReport {0}: {1}")))
            {
                for (int i = 0; i < 13; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Run Report Save hook");
            }
        }
    }
}