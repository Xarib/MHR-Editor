using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Enums;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class GreatSwordReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        foreach (var greatSword in ReaderHelper.GetWeaponData<Snow_equip_GreatSwordBaseUserData_Param>(nameof(GreatSword)).Where(gs => gs.Atk != 0))
        {
            yield return new GreatSword()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(greatSword.HyakuryuSlotNumList).ToList(),
                Attack = greatSword.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(greatSword.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(greatSword.TakumiValList).ToList(),
                Id = greatSword.Id,
                CriticalRate = greatSword.CriticalRate,
                Rarity = greatSword.RareType,
                Slots = ReaderHelper.ConvertSlots(greatSword.SlotNumList).ToList(),
                DefenseBonus = greatSword.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][greatSword.Id],
                WeaponElement = greatSword.MainElementType == Snow_equip_PlWeaponElementTypes.None ? 
                    null 
                    :
                    new WeaponElement(greatSword.MainElementType, greatSword.MainElementVal)
            };
        }
    }
}