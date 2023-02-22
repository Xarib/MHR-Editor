using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class LanceReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_LanceBaseUserData_Param>(nameof(Lance))
            .Where(lance => lance.Atk != 0)
            .Select(lance => new Lance()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(lance.HyakuryuSlotNumList).ToList(),
                Attack = lance.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(lance.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(lance.TakumiValList).ToList(),
                Id = lance.Id,
                CriticalRate = lance.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(lance.RareType),
                Slots = ReaderHelper.ConvertSlots(lance.SlotNumList).ToList(),
                DefenseBonus = lance.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][lance.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(lance.MainElementType, lance.MainElementVal),
            });

    }
}