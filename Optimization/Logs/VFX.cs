using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;

namespace Optimization.Logs
{
    public static class VFX
    {
        public static void Init()
        {
            var podGroundImpact = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/SurvivorPod/PodGroundImpact.prefab").WaitForCompletion();
            podGroundImpact.GetComponent<EffectComponent>().enabled = false;

            var spear = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIceBombGhost.prefab").WaitForCompletion();
            spear.GetComponent<EffectComponent>().enabled = false;

            var engiVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/OmniExplosionVFXEngiTurretDeath.prefab").WaitForCompletion();
            engiVFX.GetComponent<EffectComponent>().enabled = false;
        }
    }
}