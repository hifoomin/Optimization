using EntityStates.AI.Walker;
using System.Threading;
using UnityEngine;

namespace Optimization
{
    public static class Combat
    {
        private static readonly object aiLock = new();
        private static bool transitionState = false;

        public static void Combat_FixedUpdate(On.EntityStates.AI.Walker.Combat.orig_FixedUpdate orig, EntityStates.AI.Walker.Combat self)
        {
            if (self.ai && self.body)
            {
                self.aiUpdateTimer -= Time.fixedDeltaTime;
                self.strafeTimer -= Time.fixedDeltaTime;
                self.UpdateFootPosition();

                if (self.aiUpdateTimer <= 0f)
                {
                    self.aiUpdateTimer = 0.2f; // 0.2 is vanilla default

                    // new thread
                    Thread aiUpdateThread = new Thread(() =>
                    {
                        lock (aiLock) // lock to make crashing more fake
                        {
                            self.UpdateAI(0.2f);

                            if (!self.dominantSkillDriver)
                            {
                                transitionState = true;
                            }
                        }
                    });

                    aiUpdateThread.Start();
                    aiUpdateThread.Join();

                    if (transitionState)
                    {
                        self.outer.SetNextState(new LookBusy());
                        transitionState = false;
                    }
                }

                self.UpdateBark();
            }
        }
    }
}