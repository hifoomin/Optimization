using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class CombatSquad
    {
        public static void Init()
        {
            IL.RoR2.CombatSquad.FixedUpdate += CombatSquad_FixedUpdate;
        }

        private static void CombatSquad_FixedUpdate(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("CombatSquad has no living members.  Triggering defeat...")))
            {
                for (int i = 0; i < 3; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Combat Squad Fixed Update hook");
            }
        }
    }
}