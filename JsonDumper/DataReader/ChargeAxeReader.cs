using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Enums;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class ChargeAxeReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_ChargeAxeBaseUserData_Param>(nameof(ChargeAxe))
            .Where(cb => cb.Atk != 0)
            .Select(cb => new ChargeAxe()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(cb.HyakuryuSlotNumList).ToList(),
                Attack = cb.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(cb.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(cb.TakumiValList).ToList(),
                Id = cb.Id,
                CriticalRate = cb.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(cb.RareType),
                Slots = ReaderHelper.ConvertSlots(cb.SlotNumList).ToList(),
                DefenseBonus = cb.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][cb.Id],
                PhialType = cb.ChargeAxeBottleType,
                WeaponElement = ReaderHelper.ConvertWeaponElement(cb.MainElementType, cb.MainElementVal),
            });
    }
}