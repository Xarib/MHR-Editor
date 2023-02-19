using System.Collections.ObjectModel;
using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models.List_Wrappers;
using MHR_Editor.Models.Enums;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class BowReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_BowBaseUserData_Param>(nameof(Bow))
            .Where(b => b.Atk != 0)
            .Select(bow => new Bow()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(bow.HyakuryuSlotNumList).ToList(),
                Attack = bow.Atk,
                Id = bow.Id,
                CriticalRate = bow.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(bow.RareType),
                Slots = ReaderHelper.ConvertSlots(bow.SlotNumList).ToList(),
                DefenseBonus = bow.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][bow.Id],
                MaxChargeLevel = LEVEL_MAPPING[bow.BowDefaultChargeLvLimit],
                Charges = bow.BowChargeTypeList
                    .Select(wrapper => CHARGE_TYPE_MAPPING[wrapper.Value])
                    .Where(charge => charge is not null)
                    .ToList(),
                Coatings = ConvertCoatings(bow.BowBottleEquipFlagList, bow.BowBottlePowerUpTypeList).ToList(),
                WeaponElement = bow.MainElementType == Snow_equip_PlWeaponElementTypes.None
                    ? null
                    : new WeaponElement(bow.MainElementType, bow.MainElementVal)
            });
    }

    private static Dictionary<Snow_data_BowWeaponBaseData_ChageStartLvTypes, int> LEVEL_MAPPING = new()
    {
        [Snow_data_BowWeaponBaseData_ChageStartLvTypes.Lv1] = 1,
        [Snow_data_BowWeaponBaseData_ChageStartLvTypes.Lv2] = 2,
        [Snow_data_BowWeaponBaseData_ChageStartLvTypes.Lv3] = 3,
        [Snow_data_BowWeaponBaseData_ChageStartLvTypes.Lv4] = 4,
        [Snow_data_BowWeaponBaseData_ChageStartLvTypes.Max] = 4,
    };
    
    private static Dictionary<Snow_data_BowWeaponBaseData_ChargeTypes, Charge?> CHARGE_TYPE_MAPPING = new()
    {
        [Snow_data_BowWeaponBaseData_ChargeTypes.None] = null,
        [Snow_data_BowWeaponBaseData_ChargeTypes.Burst_Lv1] = new Charge()
        {
            Type = ChargeType.Rapid,
            Level = 1,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Burst_Lv2] = new Charge()
        {
            Type = ChargeType.Rapid,
            Level = 2,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Burst_Lv3] = new Charge()
        {
            Type = ChargeType.Rapid,
            Level = 3,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Burst_Lv4] = new Charge()
        {
            Type = ChargeType.Rapid,
            Level = 4,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Burst_Lv5] = new Charge()
        {
            Type = ChargeType.Rapid,
            Level = 5,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Transfix_Lv1] = new Charge()
        {
            Type = ChargeType.Pirce,
            Level = 1,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Transfix_Lv2] = new Charge()
        {
            Type = ChargeType.Pirce,
            Level = 2,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Transfix_Lv3] = new Charge()
        {
            Type = ChargeType.Pirce,
            Level = 3,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Transfix_Lv4] = new Charge()
        {
            Type = ChargeType.Pirce,
            Level = 4,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Transfix_Lv5] = new Charge()
        {
            Type = ChargeType.Pirce,
            Level = 5,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Diffusion_Lv1] = new Charge()
        {
            Type = ChargeType.Spread,
            Level = 1,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Diffusion_Lv2] = new Charge()
        {
            Type = ChargeType.Spread,
            Level = 2,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Diffusion_Lv3] = new Charge()
        {
            Type = ChargeType.Spread,
            Level = 3,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Diffusion_Lv4] = new Charge()
        {
            Type = ChargeType.Spread,
            Level = 4,
        },
        [Snow_data_BowWeaponBaseData_ChargeTypes.Diffusion_Lv5] = new Charge()
        {
            Type = ChargeType.Spread,
            Level = 5,
        },
    };

    /// <summary>
    /// 0: CloseRange
    /// 1: Power
    /// 2: Poison
    /// 3: Para
    /// 4: Sleep
    /// 5: Blast
    /// 6: Exhaust
    /// </summary>
    /// <param name="usableCoatings"></param>
    /// <param name="coatingUpgrades"></param>
    /// <returns></returns>
    private IEnumerable<Coating> ConvertCoatings(ObservableCollection<GenericWrapper<bool>> usableCoatings,
        ObservableCollection<GenericWrapper<Snow_data_BowWeaponBaseData_BottlePowerUpTypes>> coatingUpgrades)
    {
        var upgrades = new HashSet<Snow_data_BowWeaponBaseData_BottlePowerUpTypes>()
        {
            coatingUpgrades[0].Value,
            coatingUpgrades[1].Value,
        };
        
        for (var i = 0; i < usableCoatings.Count; i++)
        {
            if (!usableCoatings[i].Value)
                continue;

            var upgraded = false;

            if (UPGRADE_MAPPING.TryGetValue(i, out var type))
            {
                upgraded = upgrades.Contains(type);
            }

            yield return new Coating()
            {
                Type = i,
                Upgraded = upgraded,
            };
        }
    }

    private static Dictionary<int, Snow_data_BowWeaponBaseData_BottlePowerUpTypes> UPGRADE_MAPPING = new()
    {
        [0] = Snow_data_BowWeaponBaseData_BottlePowerUpTypes.ShortRange,
        [2] = Snow_data_BowWeaponBaseData_BottlePowerUpTypes.Poison,
        [3] = Snow_data_BowWeaponBaseData_BottlePowerUpTypes.Paralyze,
        [4] = Snow_data_BowWeaponBaseData_BottlePowerUpTypes.Sleep,
    };
}