using System.Threading;

namespace Optimization.Effect
{
    public static class UpdateTempVFX
    {
        private static readonly object overlayLock = new();

        public static void CharacterBody_UpdateAllTemporaryVisualEffects(On.RoR2.CharacterBody.orig_UpdateAllTemporaryVisualEffects orig, RoR2.CharacterBody self)
        {
            Thread overlayUpdateThread = new Thread(() =>
            {
                //lock (overlayLock) // lock to make crashing more fake
                {
                    orig(self);
                }
            });

            overlayUpdateThread.Start();
            overlayUpdateThread.Join();
        }
    }
}