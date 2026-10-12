using Alta.Networking;
using HarmonyLib;
using System.Collections.Generic;

namespace MateriaLib
{
    internal static class MaterialDurability
    {
        private static readonly HashSet<DurabilityModule> scaledModules = new HashSet<DurabilityModule>();
        private static int pruneAt = 1024;

        public static readonly HashSet<uint> vanillaMaterials = new HashSet<uint>
        {
            20178, 11420, 24094, 20186, 55232, 31502, 56394, 1, 31174, 31468, 28618, 54964, 16222, 24028, // metals
            19966, 37260, 24722, 10962, 15500 // woods
        }

        public static float GetMultiplier(PhysicalMaterial physicalMaterial)
        {
            if (physicalMaterial == null)
                return 1f;

            foreach (LibMaterial material in LibMaterial.NewMaterials)
            {
                if (material.useDurabilityMultiplier && material.physicalMaterial == physicalMaterial && physicalMaterial.DurabilityMultiplier > 0f)
                    return physicalMaterial.DurabilityMultiplier;
            }
            if (Main.vanillaDurability != null && Main.vanillaDurability.Value && vanillaMaterials.Contains(physicalMaterial.Hash) && physicalMaterial.DurabilityMultiplier > 0f)
                return physicalMaterial.DurabilityMultiplier;
            return 1f;
        }

        public static bool IsScaled(DurabilityModule module)
        {
            return module != null && scaledModules.Contains(module);
        }

        public static void Refresh(DurabilityModule module)
        {
            if (module.durabilitySettings == null || module.durabilitySettings.IsChanceBasedDurability)
                return;

            float multiplier = 1f;
            foreach (PhysicalMaterialPart part in module.GetComponentsInChildren<PhysicalMaterialPart>(true))
            {
                if (part.durabilityModule == module)
                    multiplier *= GetMultiplier(part.PhysicalMaterial);
            }

            if (multiplier == 1f && !IsScaled(module))
                return;

            float before = module.currentDurability;
            module.baseDurability = module.durabilitySettings.Durability * multiplier;
            module.ReevaluateRemainingDurabilityFromIntegrity();

            if (module.AttachedTo != null)
                module.AttachedTo.currentDurability += module.currentDurability - before;

            if (multiplier == 1f)
            {
                scaledModules.Remove(module);
            }
            else if (scaledModules.Add(module) && scaledModules.Count >= pruneAt)
            {
                scaledModules.RemoveWhere(m => m == null);
                pruneAt = scaledModules.Count * 2 + 1024;
            }
        }
    }

    [HarmonyPatch(typeof(DurabilityModule), nameof(DurabilityModule.UpdateMaterial))]
    internal static class DurabilityModuleUpdateMaterialPatch
    {
        private static void Postfix(DurabilityModule __instance, PhysicalMaterial physicalMaterial)
        {
            if (MaterialDurability.GetMultiplier(physicalMaterial) != 1f || MaterialDurability.IsScaled(__instance))
                MaterialDurability.Refresh(__instance);
        }
    }

    [HarmonyPatch(typeof(DurabilityModule), "ISavable<DurabilityModuleSaveData>.Load")]
    internal static class DurabilityModuleLoadPatch
    {
        private static void Postfix(DurabilityModule __instance)
        {
            if (MaterialDurability.IsScaled(__instance))
                MaterialDurability.Refresh(__instance);
        }
    }

    [HarmonyPatch(typeof(DurabilityModule), "Alta.Networking.IEntityReplaceHandler.ReplacedWith")]
    internal static class DurabilityModuleReplacedWithPatch
    {
        private static void Postfix(NetworkEntity entity)
        {
            Pickup pickup = entity != null ? entity.CommonPickup : null;
            DurabilityModule module = pickup != null ? pickup.DurabilityModule : null;
            if (MaterialDurability.IsScaled(module))
                MaterialDurability.Refresh(module);
        }
    }
}
