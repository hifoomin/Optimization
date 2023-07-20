using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class SetBodyPref
    {
        public static void Init()
        {
            IL.RoR2.NetworkUser.SetBodyPreference += NetworkUser_SetBodyPreference;
        }

        private static void NetworkUser_SetBodyPreference(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Changinging body preference for {0} ({1}) from {2} to {3}")))
            {
                for (int i = 0; i < 30; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Body Preference hook");
            }
        }
    }
}