using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class SetSurvivor
    {
        public static void Init()
        {
            IL.RoR2.NetworkUser.SetSurvivorPreferenceClient += NetworkUser_SetSurvivorPreferenceClient;
        }

        private static void NetworkUser_SetSurvivorPreferenceClient(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("SetSurvivorPreferenceClient survivorIndex={0}, bodyIndex={1}")))
            {
                for (int i = 0; i < 8; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Set Survivor Preference Client hook");
            }
        }
    }
}