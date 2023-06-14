using BepInEx;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using BepInEx.Logging;
using MonoMod.RuntimeDetour;
using System.Reflection;
using KinematicCharacterController;

// insanity
// insanity
// insanity
// insanity

namespace Optimization
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "Optimization";
        public const string PluginVersion = "1.0.0";

        public static ManualLogSource logger;

        public Hook updatePhase1Hook;
        public Hook updatePhase2Hook;

        public static ConfigEntry<bool> enableAIThreading { get; set; }
        public static ConfigEntry<bool> important { get; set; }
        public static ConfigEntry<bool> enableMovementThreading { get; set; }

        public void Awake()
        {
            logger = base.Logger;

            important = Config.Bind("_Important", "Everyone must have the same config in multiplayer!", true, "Everyone must have the same config in multiplayer!");
            enableAIThreading = Config.Bind("Threading", "Enable AI Threading?", true, "Tries to thread AI in combat.");
            enableMovementThreading = Config.Bind("Threading", "Enable Movement Threading?", true, "Tries to thread movement.");

            if (enableAIThreading.Value)
            {
                On.EntityStates.AI.Walker.Combat.FixedUpdate += Combat.Combat_FixedUpdate;
            }

            if (enableMovementThreading.Value)
            {
                updatePhase1Hook = new(typeof(KinematicCharacterMotor).GetMethod("UpdatePhase1"), typeof(Phase1).GetMethod("UpdatePhase1Baseder", BindingFlags.NonPublic | BindingFlags.Static));
                updatePhase2Hook = new(typeof(KinematicCharacterMotor).GetMethod("UpdatePhase2"), typeof(Phase2).GetMethod("UpdatePhase2Baseder", BindingFlags.NonPublic | BindingFlags.Static));
            }
        }
    }
}