using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class LightBowgunReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_LightBowgunBaseUserData_Param>(nameof(LightBowgun))
            .Where(lb => lb.Atk != 0)
            .Select(lb => new LightBowgun()
            {
                RampageSlots = ReaderHelper.ConvertRampageSlots(lb.HyakuryuSlotNumList).ToList(),
                Attack = lb.Atk,
                Id = lb.Id,
                CriticalRate = lb.CriticalRate,
                Rarity = ReaderHelper.ConvertRarity(lb.RareType),
                Slots = ReaderHelper.ConvertSlots(lb.SlotNumList).ToList(),
                DefenseBonus = lb.DefBonus,
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][lb.Id],
                Magazines = AmmoHelper.ConvertMagazines(
                    lb.BulletEquipFlagList,
                    lb.BulletNumList,
                    lb.BulletTypeList,
                    lb.RapidShotList
                    ).ToList(),
                Recoil = lb.Recoil,
                Fluctuation = lb.Fluctuation,
                Reload = lb.Reload,
                ClusterBombType = lb.KakusanType,
                SpecialAmmo = lb.UniqueBullet,
            });
    }
}