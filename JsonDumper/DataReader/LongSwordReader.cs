using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class LongSwordReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_LongSwordBaseUserData_Param>(nameof(LongSword))
            .Where(ls => ls.Atk != 0)
            .Select(ls => new LongSword()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(ls.HyakuryuSlotNumList).ToList(),
                Attack = ls.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(ls.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(ls.TakumiValList).ToList(),
                Id = ls.Id,
                CriticalRate = ls.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(ls.RareType),
                Slots = ReaderHelper.ConvertSlots(ls.SlotNumList).ToList(),
                DefenseBonus = ls.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][ls.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(ls.MainElementType, ls.MainElementVal),
            });
    }
}