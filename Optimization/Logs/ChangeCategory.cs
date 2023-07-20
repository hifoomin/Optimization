using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class ChangeCategory
    {
        public static void Init()
        {
            IL.RoR2.UI.LogBook.LogBookController.ChangeCategoryState.OnEnter += ChangeCategoryState_OnEnter;
        }

        private static void ChangeCategoryState_OnEnter(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("goToLastPage=true destinationPageIndex={0}")))
            {
                for (int i = 0; i < 10; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Change Category State On Enter hook");
            }
        }
    }
}