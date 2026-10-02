using Alta;
using Alta.Blacksmithing;
using Alta.Caves;
using Alta.Inventory;
using Alta.Networking;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: MelonInfo(typeof(MateriaLib.Main), "MateriaLib", "1.2.0", "Circl")]

namespace MateriaLib
{
    public class Main : MelonMod
    {
        public static event Action PreSetupMaterial = () => { };
        public static event Action SetupMaterial = () => { };
        public static event Action PostSetupMaterial = () => { };
        public static event Action PostSetupIngots = () => { };

        public static SmelterUpgrades defaultUp { get; private set; }
        public static SmelterUpgrades simpleServer { get; private set; }
        public static SmelterUpgrades gem1 { get; private set; }
        public static SmelterUpgrades gem2 { get; private set; }
        public static SmelterUpgrades gem3 { get; private set; }

        public override void OnInitializeMelon()
        {
            defaultUp = HashedGeneralValue<SmelterUpgrades>.Get(35404u);
            simpleServer = HashedGeneralValue<SmelterUpgrades>.Get(5674u);
            gem1 = HashedGeneralValue<SmelterUpgrades>.Get(23872u);
            gem2 = HashedGeneralValue<SmelterUpgrades>.Get(28650u);
            gem3 = HashedGeneralValue<SmelterUpgrades>.Get(33428u);
        }
        public override void OnLateInitializeMelon()
        {
            PreSetupMaterial?.Invoke();
            SetupMaterial?.Invoke();
            PostSetupMaterial?.Invoke();

            foreach (LibMaterial material in LibMaterial.NewMaterials)
            {
                if (material.addToList)
                {
                    try
                    {
                        HashedGeneralValue<PhysicalMaterial>.items.Add((uint)material.physicalMaterial.hash, material.physicalMaterial);
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Msg("There is a problem with a material");
                        MelonLogger.Msg($"{material.physicalMaterial.name} : {material.physicalMaterial.hash}");
                        MelonLogger.Error(ex);
                    }
                }
                if (material.distributionConfig != null)
                {
                    try
                    {
                        Distribution.Item item = new Distribution.Item();
                        item.topic = material.physicalMaterial;
                        item.baseValue = material.distributionConfig.baseValue;
                        item.noAttributeValue = material.distributionConfig.noAttributeValue;
                        item.multipliers = material.distributionConfig.multipliers.ToArray();

                        material.distributionConfig.distribution.items.Add(item);
                    }
                    catch
                    {
                        MelonLogger.Error($"Something went wrong when adding {material.physicalMaterial.name} to its distribution");
                    }
                }
            }

            MouldDefinition mould = HashedGeneralValue<MouldDefinition>.Get(22952);
            ItemSet ingots = mould.allowedMaterials;

            List<PhysicalMaterial> ListDef = defaultUp.physicalMaterials.ToList();
            List <PhysicalMaterial> ListSimp = simpleServer.physicalMaterials.ToList();
            List <PhysicalMaterial> ListGem1 = gem1.physicalMaterials.ToList();
            List <PhysicalMaterial> ListGem2 = gem2.physicalMaterials.ToList();
            List <PhysicalMaterial> ListGem3 = gem3.physicalMaterials.ToList();

            foreach (LibMaterial material in LibMaterial.NewMaterials)
            {
                if (material.ingot != null)
                {
                    ingots.items.Add(material.ingot.GetComponent<Pickup>().item);

                    switch (material.unlockAt)
                    {
                        default:
                            ListDef.Add(material.physicalMaterial);
                            break;
                        case 1:
                            ListGem1.Add(material.physicalMaterial);
                            break;
                        case 2:
                            ListGem2.Add(material.physicalMaterial);
                            break;
                        case 3:
                            ListGem3.Add(material.physicalMaterial);
                            break;
                    }

                    ListSimp.Add(material.physicalMaterial);
                }
            }

            defaultUp.physicalMaterials = ListDef.ToArray();
            simpleServer.physicalMaterials = ListSimp.ToArray();
            gem1.physicalMaterials = ListGem1.ToArray();
            gem2.physicalMaterials = ListGem2.ToArray();
            gem3.physicalMaterials = ListGem3.ToArray();

            PostSetupIngots?.Invoke();
        }
    }
}
