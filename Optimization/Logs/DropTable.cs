using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class DropTable
    {
        public static void Init()
        {
            IL.RoR2.PickupDropTable.OnEnable += PickupDropTable_OnEnable;
        }

        private static void PickupDropTable_OnEnable(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("PickupDropTable '")))
            {
                for (int i = 0; i < 6; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Pickup Drop Table On Enable hook");
            }
        }
    }
}