using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class CCSetScene
    {
        public static void Init()
        {
            IL.RoR2.Networking.NetworkManagerSystem.CCSetScene += NetworkManagerSystem_CCSetScene;
        }

        private static void NetworkManagerSystem_CCSetScene(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Setting offline scene to {0}")))
            {
                for (int i = 0; i < 8; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Network Manager System CC Set Scene hook");
            }
        }
    }
}