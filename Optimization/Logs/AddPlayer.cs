using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class AddPlayer
    {
        public static void Init()
        {
            IL.RoR2.Networking.NetworkManagerSystem.ClientAddPlayer += NetworkManagerSystem_ClientAddPlayer;
        }

        private static void NetworkManagerSystem_ClientAddPlayer(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Player {0} already added, aborting.")))
            {
                for (int i = 0; i < 9; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Network Manager System Client Add Player hook");
            }
        }
    }
}