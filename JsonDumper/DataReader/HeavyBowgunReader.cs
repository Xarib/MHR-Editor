using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class HeavyBowgunReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_HeavyBowgunBaseUserData_Param>(nameof(HeavyBowgun))
            .Where(hb => hb.Atk != 0)
            .Select(hb => new HeavyBowgun()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(hb.HyakuryuSlotNumList).ToList(),
                Attack = hb.Atk,
                Id = hb.Id,
                CriticalRate = hb.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(hb.RareType),
                Slots = ReaderHelper.ConvertSlots(hb.SlotNumList).ToList(),
                DefenseBonus = hb.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][hb.Id],
                Magazines = AmmoHelper.ConvertMagazines(
                    hb.BulletEquipFlagList,
                    hb.BulletNumList,
                    hb.BulletTypeList
                    ).ToList(),
                Recoil = hb.Recoil,
                SpecialAmmo = hb.HeavyBowgunUniqueBulletType,
                Fluctuation = hb.Fluctuation,
                Reload = hb.Reload,
                ClusterBombType = hb.KakusanType,
            });
    }
}