using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class ShortSwordReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_ShortSwordBaseUserData_Param>(nameof(ShortSword))
            .Where(gs => gs.Atk != 0)
            .Select(gs => new ShortSword()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(gs.HyakuryuSlotNumList).ToList(),
                Attack = gs.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(gs.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(gs.TakumiValList).ToList(),
                Id = gs.Id,
                CriticalRate = gs.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(gs.RareType),
                Slots = ReaderHelper.ConvertSlots(gs.SlotNumList).ToList(),
                DefenseBonus = gs.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][gs.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(gs.MainElementType, gs.MainElementVal),
            });
    }
}