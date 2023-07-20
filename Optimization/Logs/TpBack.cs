using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class TpBack
    {
        public static void Init()
        {
            IL.RoR2.MapZone.TeleportBody += MapZone_TeleportBody;
        }

        private static void MapZone_TeleportBody(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("tp back")))
            {
                c.Remove();
                c.Remove();
            }
            else
            {
                Main.logger.LogError("Failed to apply Map Zone Teleport Body hook");
            }
        }
    }
}