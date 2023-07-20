using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class PlayCrossfade
    {
        public static void Init()
        {
            IL.EntityStates.EntityState.PlayCrossfade_string_string_string_float_float += EntityState_PlayCrossfade_string_string_string_float_float;
        }

        private static void EntityState_PlayCrossfade_string_string_string_float_float(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("EntityState.PlayCrossfade: Zero duration is not allowed. type={0}")))
            {
                for (int i = 0; i < 10; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Play Crossfade hook");
            }
        }
    }
}