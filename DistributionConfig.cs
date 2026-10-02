using Alta;
using Alta.Caves;
using MelonLoader;
using System.Collections.Generic;

namespace MateriaLib
{
    public class DistributionConfig
    {
        public Distribution distribution;
        public float baseValue;
        public float noAttributeValue;
        public readonly List<AttributeCurveRange> multipliers = new List<AttributeCurveRange>();

        public static DistributionConfig CreateDefault(float BaseValue, float NoAttributeValue, LibMaterial.MaterialType type, float[] values)
        {
            switch (type)
            {
                default:
                    MelonLogger.Error("Somehow type wasn't a MaterialType");
                    return null;
                case LibMaterial.MaterialType.metal:
                    return new DistributionConfig(BaseValue, NoAttributeValue, HashedGeneralValue<Distribution>.Get(43946u));
                case LibMaterial.MaterialType.leather:
                    return new DistributionConfig(BaseValue, NoAttributeValue, HashedGeneralValue<Distribution>.Get(49220u));
                case LibMaterial.MaterialType.wood:
                    MelonLogger.Error("Sorry, theres no wood material distribution :(");
                    return null;
                case LibMaterial.MaterialType.canvas:
                    MelonLogger.Error("Sorry, theres no canvas material distribution :(");
                    return null;
                    
            }
        }

        public DistributionConfig(float BaseValue, float NoAttributeValue, Distribution ThisDistribution)
        {
            distribution = ThisDistribution;
            baseValue = BaseValue;
            noAttributeValue = NoAttributeValue;
        }
    }
}
