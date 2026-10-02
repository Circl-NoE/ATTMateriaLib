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

        public static DistributionConfig CreateDefault(float BaseValue, float NoAttributeValue, LibMaterial.MaterialType type)
        {
            switch (type)
            {
                default:
                    MelonLogger.Error("Sorry, there is no vanilla distribution for that type");
                    return null;
                case LibMaterial.MaterialType.metal:
                    return new DistributionConfig(BaseValue, NoAttributeValue, HashedGeneralValue<Distribution>.Get(43946u));
                case LibMaterial.MaterialType.leather:
                    return new DistributionConfig(BaseValue, NoAttributeValue, HashedGeneralValue<Distribution>.Get(49220u));
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
