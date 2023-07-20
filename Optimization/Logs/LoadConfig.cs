using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class LoadConfig
    {
        public static void Init()
        {
            IL.RoR2.Console.LoadConfig += Console_LoadConfig;
        }

        private static void Console_LoadConfig(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Could not load config {0}: {1}")))
            {
                for (int i = 0; i < 14; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Console Load Config hook");
            }
        }
    }
}