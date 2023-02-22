using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class InsectGlaiveReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_InsectGlaiveBaseUserData_Param>(nameof(InsectGlaive))
            .Where(ig => ig.Atk != 0)
            .Select(ig => new InsectGlaive()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(ig.HyakuryuSlotNumList).ToList(),
                Attack = ig.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(ig.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(ig.TakumiValList).ToList(),
                Id = ig.Id,
                CriticalRate = ig.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(ig.RareType),
                Slots = ReaderHelper.ConvertSlots(ig.SlotNumList).ToList(),
                DefenseBonus = ig.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][ig.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(ig.MainElementType, ig.MainElementVal),
                InsectLevel = (int)ig.InsectGlaiveInsectLv + 1,
            });
    }
}