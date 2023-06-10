using BepInEx;
using RoR2;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using UnityEngine;
using System;

namespace Optimization
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "Optimization";
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            IL.RoR2.CharacterMotor.PreMove += CharacterMotor_PreMove;
        }

        private void CharacterMotor_PreMove(ILContext il)
        {
            ILCursor c = new(il);

            ILLabel ret = null;
            ILLabel firstLdloc = null;
            ILLabel firstLdarg0 = null;
            ILLabel firstLdcR4 = null;
            ILLabel firstMul = null;
            ILLabel firstLdlocas = null;
            ILLabel secondLdarg0 = null;
            ILLabel secondLdloc = null;

            while (c.Next != null)
            {
                c.Remove();
            }

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_hasEffectiveAuthority"));

            c.Emit(OpCodes.Ret);
            c.Index--;
            ret = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brfalse, ret);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_acceleration"));
            c.Emit(OpCodes.Stloc_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(CharacterMotor).GetField("isAirControlForced"));

            c.Emit(OpCodes.Ldloc_0);
            c.Index--;
            firstLdloc = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brtrue_S, firstLdloc);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_isGrounded"));

            c.Emit(OpCodes.Ldarg_0);
            c.Index--;
            firstLdarg0 = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brtrue_S, firstLdarg0);
            // c.Emit(OpCodes.Ldloc_0); this is firstLdloc
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(CharacterMotor).GetField("disableAirControlUntilCollision"));

            c.Emit(OpCodes.Ldc_R4, 0.0f);
            c.Index--;
            firstLdcR4 = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brtrue_S, firstLdcR4);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(CharacterMotor).GetField("airControl"));

            c.Emit(OpCodes.Mul);
            c.Index--;
            firstMul = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Br_S, firstMul);
            // c.Emit(OpCodes.Ldc_R4, 0.0f); this is firstLdcR4
            // c.Emit(OpCodes.Mul); this is firstMul
            c.Emit(OpCodes.Stloc_0);
            // c.Emit(OpCodes.Ldarg_0); this is firstLdarg0
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_moveDirection"));
            c.Emit(OpCodes.Stloc_1);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_isFlying"));

            c.Emit(OpCodes.Ldloca_S, 1);
            c.Index--;
            firstLdlocas = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brtrue_S, firstLdlocas);
            c.Emit(OpCodes.Ldloca_S, 1);
            c.Emit(OpCodes.Ldloc_1);
            c.Emit(OpCodes.Ldfld, typeof(Vector3).GetField("x"));
            c.Emit(OpCodes.Ldc_R4, 0.0f);
            c.Emit(OpCodes.Ldloc_1);
            c.Emit(OpCodes.Ldfld, typeof(Vector3).GetField("z"));
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod(".ctor", new Type[] { typeof(float), typeof(float), typeof(float) }));
            // c.Emit(OpCodes.Ldloca_S, 1); this is firstLdlocas
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("get_sqrMagnitude"));
            c.Emit(OpCodes.Ldc_R4, 1f);

            c.Emit(OpCodes.Ldarg_0);
            c.Index--;
            secondLdarg0 = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Bge_Un_S, secondLdarg0);
            c.Emit(OpCodes.Ldloca_S, 1);
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("get_sqrMagnitude"));
            c.Emit(OpCodes.Ldc_R4, 0.0f);
            c.Emit(OpCodes.Ble_Un_S, secondLdarg0);
            c.Emit(OpCodes.Ldloca_S, 1);
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("Normalize"));
            // c.Emit(OpCodes.Ldarg_0); this is secondLdarg0
            c.Emit(OpCodes.Ldfld, typeof(CharacterMotor).GetField("body"));
            c.Emit(OpCodes.Callvirt, typeof(CharacterBody).GetMethod("get_isSprinting"));

            c.Emit(OpCodes.Ldloc_1);
            c.Index--;
            secondLdloc = c.MarkLabel();
            c.MoveBeforeLabels();

            c.Emit(OpCodes.Brfalse_S, secondLdloc);
            c.Emit(OpCodes.Ldloca_S, 1);
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("get_magnitude"));
            c.Emit(OpCodes.Stloc_3);
            c.Emit(OpCodes.Ldloc_3);
            c.Emit(OpCodes.Ldc_R4, 1f);
            c.Emit(OpCodes.Bge_Un_S, secondLdloc);
            c.Emit(OpCodes.Ldloc_3);
            c.Emit(OpCodes.Ldc_R4, 0.0f);
            c.Emit(OpCodes.Ble_Un_S, secondLdloc);
            c.Emit(OpCodes.Ldloc_1);
            c.Emit(OpCodes.Ldc_R4, 1f);
            c.Emit(OpCodes.Ldloc_3);
            c.Emit(OpCodes.Div);
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("op_Multiply", new Type[] { typeof(Vector3), typeof(float) }));
            c.Emit(OpCodes.Stloc_1);
            // c.Emit(OpCodes.Ldloc_1); this is secondLdloc
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_walkSpeed"));
            c.Emit(OpCodes.Call, typeof(Vector3).GetMethod("op_Multiply", new Type[] { typeof(Vector3), typeof(float) }));
            c.Emit(OpCodes.Stloc_2);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Call, typeof(CharacterMotor).GetMethod("get_isFlying"));
            // OptimizedRoR2.dll IL_00ca: c.Emit(OpCodes.
        }
    }
}