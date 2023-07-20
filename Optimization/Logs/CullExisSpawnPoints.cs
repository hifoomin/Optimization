using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class CullExisSpawnPoints
    {
        public static void Init()
        {
            IL.RoR2.SceneDirector.CullExistingSpawnPoints += SceneDirector_CullExistingSpawnPoints;
        }

        private static void SceneDirector_CullExistingSpawnPoints(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("reorder list")))
            {
                for (int i = 0; i < 2; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Scene Director Cull Existing Spawn Points hook");
            }
        }
    }
}