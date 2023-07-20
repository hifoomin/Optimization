using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class SaveArchiveCV
    {
        public static void Init()
        {
            IL.RoR2.Console.SaveArchiveConVars += Console_SaveArchiveConVars;
        }

        private static void Console_SaveArchiveConVars(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdloc(out _),
                x => x.MatchLdstr("echo \"Loaded archived convars.\";")))
            {
                for (int i = 0; i < 3; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Save Archive Con Vars hook");
            }
        }
    }
}