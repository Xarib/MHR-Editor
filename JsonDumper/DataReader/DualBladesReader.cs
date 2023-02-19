using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Enums;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class DualBladesReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_DualBladesBaseUserData_Param>(nameof(DualBlades))
            .Where(db => db.Atk != 0)
            .Select(db => new DualBlades()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(db.HyakuryuSlotNumList).ToList(),
                Attack = db.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(db.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(db.TakumiValList).ToList(),
                Id = db.Id,
                CriticalRate = db.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(db.RareType),
                Slots = ReaderHelper.ConvertSlots(db.SlotNumList).ToList(),
                DefenseBonus = db.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][db.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(db.MainElementType, db.MainElementVal),
                AdditionalElement = ReaderHelper.ConvertWeaponElement(db.SubElementType, db.SubElementVal),
            });
    }
}