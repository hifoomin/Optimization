using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class DropRewards
    {
        public static void Init()
        {
            IL.RoR2.BossGroup.DropRewards += BossGroup_DropRewards;
        }

        private static void BossGroup_DropRewards(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("No valid run instance!")))
            {
                for (int i = 0; i < 2; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Boss Group Drop Rewards hook");
            }
        }
    }
}