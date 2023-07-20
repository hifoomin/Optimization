using MonoMod.Cil;

namespace Optimization.Logs
{
    public static class MainMenu
    {
        public static void Init()
        {
            IL.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += BaseMainMenuScreen_OnEnter;
        }

        private static void BaseMainMenuScreen_OnEnter(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("BaseMainMenuScreen: OnEnter()")))
            {
                for (int i = 0; i < 3; i++)
                {
                    c.Remove();
                }
            }
            else
            {
                Main.logger.LogError("Failed to apply Base Main Menu Screen On Enter hook");
            }
        }
    }
}