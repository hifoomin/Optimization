using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class PlayAnimation
    {
        public static void Init()
        {
            IL.EntityStates.EntityState.PlayAnimation_string_string_string_float += EntityState_PlayAnimation_string_string_string_float;
        }

        private static void EntityState_PlayAnimation_string_string_string_float(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("EntityState.PlayAnimation: Zero duration is not allowed. type={0}")))
            {
                for (int i = 0; i < 10; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Play Animation hook");
            }
        }
    }
}