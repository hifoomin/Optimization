using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class OSP
    {
        public static void Init()
        {
            IL.RoR2.HealthComponent.TriggerOneShotProtection += HealthComponent_TriggerOneShotProtection;
        }

        private static void HealthComponent_TriggerOneShotProtection(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("OSP Triggered.")))
            {
                for (int i = 0; i < 2; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply OSP hook");
            }
        }
    }
}