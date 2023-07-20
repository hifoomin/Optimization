using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class Handlers
    {
        public static void Init()
        {
            IL.RoR2.Networking.NetworkMessageHandlerAttribute.CollectHandlers += NetworkMessageHandlerAttribute_CollectHandlers;
        }

        private static void NetworkMessageHandlerAttribute_CollectHandlers(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Network message MsgType.Highest + {0} is unregistered.")))
            {
                for (int i = 0; i < 11; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Network Message Handler Attribute Collect Handlers hook");
            }
        }
    }
}