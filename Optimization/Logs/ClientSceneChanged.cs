using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class ClientSceneChanged
    {
        public static void Init()
        {
            IL.RoR2.Networking.NetworkManagerSystem.OnClientSceneChanged += NetworkManagerSystem_OnClientSceneChanged;
        }

        private static void NetworkManagerSystem_OnClientSceneChanged(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("OnClientSceneChanged networkSceneName=")))
            {
                for (int i = 0; i < 8; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Network Manager System On Client Scene Changed hook");
            }
        }
    }
}