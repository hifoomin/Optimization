using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class CombatDirector
    {
        public static void Init()
        {
            IL.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
        }

        private static void CombatDirector_Spawn(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Spawn card {0} failed to spawn. Aborting cost procedures.")))
            {
                for (int i = 0; i < 9; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Combat Director Spawn hook");
            }
        }
    }
}