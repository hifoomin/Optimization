using RoR2.UI.SkinControllers;

namespace Optimization.Logs
{
    public static class Button
    {
        public static void Init()
        {
            On.RoR2.UI.SkinControllers.ButtonSkinController.StaticUpdate += ButtonSkinController_StaticUpdate;
        }

        private static void ButtonSkinController_StaticUpdate(On.RoR2.UI.SkinControllers.ButtonSkinController.orig_StaticUpdate orig)
        {
            foreach (ButtonSkinController buttonSkinController in ButtonSkinController.instancesList)
            {
                if (buttonSkinController && buttonSkinController.skinData)
                    buttonSkinController.UpdateLabelStyle(ref buttonSkinController.skinData.buttonStyle);
            }
        }
    }
}