using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Enums;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class GunLanceReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_GunLanceBaseUserData_Param>(nameof(GunLance))
            .Where(gl => gl.Atk != 0)
            .Select(gl => new GunLance()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(gl.HyakuryuSlotNumList).ToList(),
                Attack = gl.Atk,
                Sharpness = ReaderHelper.ConvertSharpness(gl.SharpnessValList).ToList(),
                Handicraft = ReaderHelper.ConvertHandicraft(gl.TakumiValList).ToList(),
                Id = gl.Id,
                CriticalRate = gl.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(gl.RareType),
                Slots = ReaderHelper.ConvertSlots(gl.SlotNumList).ToList(),
                DefenseBonus = gl.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][gl.Id],
                WeaponElement = ReaderHelper.ConvertWeaponElement(gl.MainElementType, gl.MainElementVal),
                ShellingType = gl.GunLanceFireType,
                ShellingLevel = ConvertShellingLevel(gl.GunLanceFireLv),
            });
    }

    private static int ConvertShellingLevel(Snow_data_GunLanceFireData_GunLanceFireLv level)
        => ((int)level) + 1;
}