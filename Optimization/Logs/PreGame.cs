using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class PreGame
    {
        public static void Init()
        {
            IL.RoR2.PreGameController.Start += PreGameController_Start;
        }

        private static void PreGameController_Start(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Attempting to generate PreGameVoteController for {0}")))
            {
                for (int i = 0; i < 9; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Pre Game Controller Start hook");
            }
        }
    }
}