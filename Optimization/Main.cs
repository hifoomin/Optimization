using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MonoMod.RuntimeDetour;
using System.Reflection;
using KinematicCharacterController;
using Optimization.Logs;
using Optimization.AI;
using Optimization.KinematicCM;
using Optimization.Effect;
using CombatDirector = Optimization.Logs.CombatDirector;
using CombatSquad = Optimization.Logs.CombatSquad;

namespace Optimization
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "Optimization";
        public const string PluginVersion = "1.1.0";

        public static ManualLogSource logger;

        public Hook updatePhase1Hook;
        public Hook updatePhase2Hook;
        public Hook preSimHook;
        public Hook simHook;
        public Hook postSimHook;

        public static ConfigEntry<bool> enableAIThreading { get; set; }
        public static ConfigEntry<bool> important { get; set; }

        public static ConfigEntry<bool> enableMovementThreading { get; set; }
        public static ConfigEntry<bool> enableOverlayThreading { get; set; }

        // notes
        // thread with lock and start, but no join => usually worse perf
        // thread with lock and start and join => usually worse perf
        // task => delayed/instant crash
        // threadpool => instant crash/break
        // async await task => delayed crash
        // parallel for => heavily depends
        // jobs => no

        public void Awake()
        {
            // I know c.remove is a crime, but I can't be bothered to learn skipping branches rn

            logger = base.Logger;

            important = Config.Bind("_Important", "Everyone must have the same config in multiplayer!", true, "Everyone must have the same config in multiplayer!");
            enableAIThreading = Config.Bind("Threading", "Enable AI Threading?", true, "Tries to thread AI in combat.");
            enableMovementThreading = Config.Bind("Threading", "Enable Movement Threading?", true, "Tries to thread movement.");
            enableOverlayThreading = Config.Bind("Threading", "Enable Overlay Threading?", true, "Tries to thread overlays.");
            AddNode.Init();
            AddPlayer.Init();
            BuildCategories.Init();
            Button.Init();
            CCSetScene.Init();
            ChangeCategory.Init();
            ClientSceneChanged.Init();
            CombatDirector.Init();
            CombatSquad.Init();
            CullExisSpawnPoints.Init();
            DropRewards.Init();
            DropTable.Init();
            Handlers.Init();
            Load.Init();
            LoadConfig.Init();
            LoadRunReport.Init();
            LoadUserProf.Init();
            MainMenu.Init();
            OSP.Init();
            PlayAnimation.Init();
            PlayCrossfade.Init();
            PreGame.Init();
            Save.Init();
            SaveArchiveCV.Init();
            SetBodyPref.Init();
            SetSurvivor.Init();
            StatSheet.Init();
            TpBack.Init();
            VFX.Init();

            if (enableAIThreading.Value)
            {
                On.EntityStates.AI.Walker.Combat.FixedUpdate += Combat.Combat_FixedUpdate;
            }

            if (enableOverlayThreading.Value)
            {
                On.RoR2.CharacterBody.UpdateAllTemporaryVisualEffects += UpdateTempVFX.CharacterBody_UpdateAllTemporaryVisualEffects;
            }

            if (enableMovementThreading.Value)
            {
                updatePhase1Hook = new(typeof(KinematicCharacterMotor).GetMethod("UpdatePhase1"), typeof(Phase1).GetMethod("UpdatePhase1Baseder", BindingFlags.NonPublic | BindingFlags.Static));
                updatePhase2Hook = new(typeof(KinematicCharacterMotor).GetMethod("UpdatePhase2"), typeof(Phase2).GetMethod("UpdatePhase2Baseder", BindingFlags.NonPublic | BindingFlags.Static));
            }
        }
    }
}