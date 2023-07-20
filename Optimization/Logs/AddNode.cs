using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class AddNode
    {
        public static void Init()
        {
            IL.RoR2.ViewablesCatalog.AddNodeToRoot += ViewablesCatalog_AddNodeToRoot;
        }

        private static void ViewablesCatalog_AddNodeToRoot(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Tried to add duplicate node {0}")))
            {
                for (int i = 0; i < 9; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Viewables Catalog Add Note To Root hook");
            }
        }
    }
}