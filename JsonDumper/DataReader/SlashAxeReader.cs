using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class SlashAxeReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_SlashAxeBaseUserData_Param>(nameof(SlashAxe))
            .Where(sa => sa.Atk != 0)
            .Select(sa => new SlashAxe()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(sa.HyakuryuSlotNumList).ToList(),
                Attack = sa.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(sa.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(sa.TakumiValList).ToList(),
                Id = sa.Id,
                CriticalRate = sa.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(sa.RareType),
                Slots = ReaderHelper.ConvertSlots(sa.SlotNumList).ToList(),
                DefenseBonus = sa.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][sa.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(sa.MainElementType, sa.MainElementVal),
                PhialType = sa.SlashAxeBottleType,
                PhialElement = sa.SlashAxeBottleElementVal,
            });
    }
}