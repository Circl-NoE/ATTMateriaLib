using Alta;
using System.Collections.Generic;
using UnityEngine;

namespace MateriaLib
{
    public class LibMaterial
    {
        public enum MaterialType
        {
            metal = 1, //iron
            wood = 24722, //oak
            leather = 33384, //dais leather
            canvas = 61790, //canvas
            rope = 35204 //rope
        }
        
        public PhysicalMaterial physicalMaterial { get; private set; }
        public GameObject ingot = null;
        public int unlockAt = 0;
        public bool addToList = true;
        public DistributionConfig distributionConfig = null;
        public LibMaterial(string name, int hash)
        {
            physicalMaterial = UnityEngine.Object.Instantiate(HashedGeneralValue<PhysicalMaterial>.Get((uint)MaterialType.metal));
            physicalMaterial.name = name;
            physicalMaterial.hash = hash;
        }
        public LibMaterial(string name, int hash, MaterialType matType)
        {
            uint basehash = (uint)matType;
            physicalMaterial = UnityEngine.Object.Instantiate(HashedGeneralValue<PhysicalMaterial>.Get(basehash));
            physicalMaterial.name = name;
            physicalMaterial.hash = hash;
        }
        public LibMaterial(string name, int hash, PhysicalMaterial physMat)
        {
            physicalMaterial = physMat;
            physicalMaterial.name = name;
            physicalMaterial.hash = hash;
        }
        public LibMaterial(PhysicalMaterial physMat)
        {
            physicalMaterial = physMat;
        }
        public void Configure(MaterialConfig config)
        {
            if (config.SourceThermalConductivity != null)
                physicalMaterial.sourceThermalConductivity = config.SourceThermalConductivity.Value;

            if (config.RecieveThermalConductivity != null)
                physicalMaterial.receiveThermalConductivity = config.RecieveThermalConductivity.Value;

            if (config.InternalThermalConductivity != null)
                physicalMaterial.internalThermalConductivity = config.InternalThermalConductivity.Value;

            if (config.GlowingStart != null)
                physicalMaterial.glowingStart = config.GlowingStart.Value;

            if (config.GlowingEnd != null)
                physicalMaterial.glowingEnd = config.GlowingEnd.Value;

            if (config.ForgeMultiplier != null)
                physicalMaterial.forgeMultiplier = config.ForgeMultiplier.Value;

            if (config.MeltingPoint != null)
                physicalMaterial.meltingPoint = config.MeltingPoint.Value;

            if (config.MaxTemperatureForParticles != null)
                physicalMaterial.maxTemperatureForParticles = config.MaxTemperatureForParticles.Value;

            if (config.NailHealthMultiplier != null)
                physicalMaterial.nailHealthMultiplier = config.NailHealthMultiplier.Value;

            if (config.MaxCraftingDamageMultiplier != null)
                physicalMaterial.maxCraftingDamageMultiplier = config.MaxCraftingDamageMultiplier.Value;

            if (config.DamageMultiplier != null)
                physicalMaterial.damageMultiplier = config.DamageMultiplier.Value;

            if (config.DurabilityMultiplier != null)
                physicalMaterial.durabilityMultiplier = config.DurabilityMultiplier.Value;

            if (config.Density != null)
                physicalMaterial.density = config.Density.Value;

            if (config.WeightMultiplier != null)
                physicalMaterial.weightMultiplier = config.WeightMultiplier.Value;

            if (config.TightnessMultiplier != null)
                physicalMaterial.tightnessMultiplier = config.TightnessMultiplier.Value;

            if (config.TightnessBowImpact != null)
                physicalMaterial.tightnessBowImpact = config.TightnessBowImpact.Value;

            if (config.MinimumProjectileWeight != null)
                physicalMaterial.minimumProjectileWeight = config.MinimumProjectileWeight.Value;

            if (config.MaxInvalidProjectileVelocityMultiplier != null)
                physicalMaterial.maxInvalidProjectileVelocityMultiplier = config.MaxInvalidProjectileVelocityMultiplier.Value;

            if (config.NoiseMultiplier != null)
                physicalMaterial.noiseMultiplier = config.NoiseMultiplier.Value;

            if (config.HardnessLevel != null)
                physicalMaterial.hardnessLevel = config.HardnessLevel.Value;

        }
        public void ReplaceAllMaterials(Material[] ingot, Material cauldron) // for metal
        {
            ReplaceMaterialsInChannel(0, ingot[0], ingot[1]);
            ReplaceMaterialsInChannel(1, cauldron);
        }
        public void ReplaceAllMaterials(Material[] wood, Material charred, Material burnt, Material ashen) // for wood
        {
            ReplaceMaterialsInChannel(0, wood[0], wood[1]);
            ReplaceMaterialsInChannel(1, charred);
            ReplaceMaterialsInChannel(1, burnt);
            ReplaceMaterialsInChannel(1, ashen);
        }
        public void ReplaceAllMaterials(Material material) // for leather or rope
        {
            ReplaceMaterialsInChannel(0, material);
        }
        public void ReplaceAllMaterials(Material worn, Material cutout) // for canvas
        {
            ReplaceMaterialsInChannel(0, worn);
            ReplaceMaterialsInChannel(1, cutout);
        }
        public void ReplaceMaterialsInChannel(int channel, Material visMaterial)
        {
            ReplaceMaterialsInChannel(channel, visMaterial, visMaterial);
        }
        public void ReplaceMaterialsInChannel(int channel, Material visMaterial, Material visMaterial2)
        {
            physicalMaterial.materialChannels[channel].material = visMaterial;
            physicalMaterial.materialChannels[channel].atlasedMaterial = visMaterial2;
            physicalMaterial.materialChannels[channel].nonDeformingMaterial = visMaterial2;
        }

        public Material GetMaterialFromChannelSpot(int channel, int spot)
        {
            switch (spot)
            {
                default:
                    return physicalMaterial.materialChannels[channel].material;
                case 2:
                    return physicalMaterial.materialChannels[channel].atlasedMaterial;
                case 3:
                    return physicalMaterial.materialChannels[channel].nonDeformingMaterial;
            }
        }

        public static readonly List<LibMaterial> NewMaterials = new List<LibMaterial>();
    }
}
