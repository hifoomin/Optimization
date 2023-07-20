using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class StatSheet
    {
        public static void Init()
        {
            IL.RoR2.Stats.StatSheet.Init += StatSheet_Init;
        }

        private static void StatSheet_Init(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("init stat sheet")))
            {
                for (int i = 0; i < 2; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Stat Sheet Init hook");
            }
        }
    }
}