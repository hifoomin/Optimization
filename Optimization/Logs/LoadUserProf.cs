using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class LoadUserProf
    {
        public static void Init()
        {
            IL.RoR2.SaveSystemSteam.LoadUserProfileFromDisk += SaveSystemSteam_LoadUserProfileFromDisk;
        }

        private static void SaveSystemSteam_LoadUserProfileFromDisk(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Attempting to load user profile {0}")))
            {
                for (int i = 0; i < 9; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Save System Steam Load User Profile From Disk 1 hook");
            }
            /*
            c.Index = 0;

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("stream.Length={0}")))
            {
                for (int i = 0; i < 10; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Save System Steam Load User Profile From Disk 2 hook");
            }
            */
        }
    }
}