using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class HornReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_HornBaseUserData_Param>(nameof(Horn))
            .Where(hh => hh.Atk != 0)
            .Select(hh => new Horn()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(hh.HyakuryuSlotNumList).ToList(),
                Attack = hh.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(hh.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(hh.TakumiValList).ToList(),
                Id = hh.Id,
                CriticalRate = hh.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(hh.RareType),
                Slots = ReaderHelper.ConvertSlots(hh.SlotNumList).ToList(),
                DefenseBonus = hh.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][hh.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(hh.MainElementType, hh.MainElementVal),
                MelodyEffects = hh.HornMelodyTypeList.Select(wr => wr.Value).ToList(),
            });
    }
}