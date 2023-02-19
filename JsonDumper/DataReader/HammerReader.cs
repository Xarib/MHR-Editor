using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class HammerReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_HammerBaseUserData_Param>(nameof(Hammer))
            .Where(hm => hm.Atk != 0)
            .Select(hm => new Hammer()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(hm.HyakuryuSlotNumList).ToList(),
                Attack = hm.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(hm.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(hm.TakumiValList).ToList(),
                Id = hm.Id,
                CriticalRate = hm.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(hm.RareType),
                Slots = ReaderHelper.ConvertSlots(hm.SlotNumList).ToList(),
                DefenseBonus = hm.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][hm.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(hm.MainElementType, hm.MainElementVal),
            });
    }
}