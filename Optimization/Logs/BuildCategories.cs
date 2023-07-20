using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class BuildCategories
    {
        public static void Init()
        {
            IL.RoR2.UI.LogBook.LogBookController.BuildCategoriesButtons += LogBookController_BuildCategoriesButtons;
        }

        private static void LogBookController_BuildCategoriesButtons(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Building category buttons.")))
            {
                for (int i = 0; i < 2; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Log Book Controller Build Categories Buttons hook");
            }
        }
    }
}