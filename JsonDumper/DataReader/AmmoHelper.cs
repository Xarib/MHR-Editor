using System.Collections.ObjectModel;
using MHR_Editor.Common.Models.List_Wrappers;
using MHR_Editor.Models.Enums;

namespace JsonDumper.DataReader;

public static class AmmoHelper
{
    private static Dictionary<int, (AmmoType, int?)> AMMO_TYPE_INDEX_MAP = new()
    {
        [0] = (AmmoType.Unknown, null),
        [1] = (AmmoType.Normal, 1),
        [2] = (AmmoType.Normal, 2),
        [3] = (AmmoType.Normal, 3),
        [4] = (AmmoType.Pierce, 1),
        [5] = (AmmoType.Pierce, 2),
        [6] = (AmmoType.Pierce, 3),
        [7] = (AmmoType.Spread, 1),
        [8] = (AmmoType.Spread, 2),
        [9] = (AmmoType.Spread, 3),
        [10] = (AmmoType.Shrapnel, 1),
        [11] = (AmmoType.Shrapnel, 2),
        [12] = (AmmoType.Shrapnel, 3),
        [13] = (AmmoType.Sticky, 1),
        [14] = (AmmoType.Sticky, 2),
        [15] = (AmmoType.Sticky, 3),
        [16] = (AmmoType.Cluster, 1),
        [17] = (AmmoType.Cluster, 2),
        [18] = (AmmoType.Cluster, 3),
        [19] = (AmmoType.Poison, 1),
        [20] = (AmmoType.Poison, 2),
        [21] = (AmmoType.Paralysis, 1),
        [22] = (AmmoType.Paralysis, 2),
        [23] = (AmmoType.Sleep, 1),
        [24] = (AmmoType.Sleep, 2),
        [25] = (AmmoType.Exhaust, 1),
        [26] = (AmmoType.Exhaust, 2),
        [27] = (AmmoType.Recover, 1),
        [28] = (AmmoType.Recover, 2),
        [29] = (AmmoType.Demon, null),
        [30] = (AmmoType.Armor, null),
        [31] = (AmmoType.Flaming, null),
        [32] = (AmmoType.FlamingPierce, null),
        [33] = (AmmoType.Water, null),
        [34] = (AmmoType.WaterPierce, null),
        [35] = (AmmoType.Freeze, null),
        [36] = (AmmoType.FreezePierce, null),
        [37] = (AmmoType.Thunder, null),
        [38] = (AmmoType.ThunderPierce, null),
        [39] = (AmmoType.Dragon, null),
        [40] = (AmmoType.DragonPierce, null),
        [41] = (AmmoType.Slicing, null),
        [42] = (AmmoType.Wyvern, null),
        [43] = (AmmoType.Tranq, null),
        [44] = (AmmoType.Unknown, null),
        [45] = (AmmoType.Unknown, null),
        [46] = (AmmoType.Unknown, null),
        [47] = (AmmoType.Unknown, null),
        [48] = (AmmoType.Unknown, null),
        [49] = (AmmoType.Unknown, null),
        [50] = (AmmoType.Unknown, null),
        [51] = (AmmoType.Unknown, null),
    };

    public static IEnumerable<HeavyBowgunMagazine> ConvertMagazines(
        ObservableCollection<GenericWrapper<bool>> bulletType,
        ObservableCollection<GenericWrapper<uint>> capacity,
        ObservableCollection<GenericWrapper<Snow_data_GameItemEnum_ShootType>> shootType
        )
    {
        for (var i = 0; i < bulletType.Count; i++)
        {
            if (!bulletType[i].Value)
                continue;
            
            var (ammoType, ammoSize) = AMMO_TYPE_INDEX_MAP[i];
            
            if (ammoType == AmmoType.Unknown)
                continue;

            yield return new HeavyBowgunMagazine()
            {
                AmmoSize = ammoSize,
                AmmoType = ammoType,
                Capacity = capacity[i].Value,
                ShootType = shootType[i].Value,
            };
        }
    }
    public static IEnumerable<LightBowgunMagazine> ConvertMagazines(
        ObservableCollection<GenericWrapper<bool>> bulletType,
        ObservableCollection<GenericWrapper<uint>> capacity,
        ObservableCollection<GenericWrapper<Snow_data_GameItemEnum_ShootType>> shootType,
        ObservableCollection<GenericWrapper<Snow_data_GameItemEnum_BulletType>> rapidShotList
        )
    {
        var rapidShotAmmo = rapidShotList
            .Select(wr => (int)wr.Value)
            .ToHashSet();
        
        for (var i = 0; i < bulletType.Count; i++)
        {
            if (!bulletType[i].Value)
                continue;
            
            var (ammoType, ammoSize) = AMMO_TYPE_INDEX_MAP[i];
            
            if (ammoType == AmmoType.Unknown)
                continue;

            yield return new LightBowgunMagazine()
            {
                AmmoSize = ammoSize,
                AmmoType = ammoType,
                Capacity = capacity[i].Value,
                ShootType = shootType[i].Value,
                Rapid = rapidShotAmmo.Contains(i),
            };
        }
    }
}

public class HeavyBowgunMagazine
{
    public AmmoType AmmoType { get; set; }
    public int? AmmoSize { get; set; }
    public uint Capacity { get; set; }
    public Snow_data_GameItemEnum_ShootType ShootType { get; set; }
}
public class LightBowgunMagazine
{
    public AmmoType AmmoType { get; set; }
    public int? AmmoSize { get; set; }
    public uint Capacity { get; set; }
    public Snow_data_GameItemEnum_ShootType ShootType { get; set; }
    public bool Rapid { get; set; }
}

public enum AmmoType
{
    Unknown,
    Normal,
    Pierce,
    Spread,
    Shrapnel,
    Sticky,
    Cluster,
    Recover,
    Poison,
    Paralysis,
    Sleep,
    Exhaust,
    Flaming,
    Water,
    Thunder,
    Freeze,
    Dragon,
    FlamingPierce,
    WaterPierce,
    ThunderPierce,
    FreezePierce,
    DragonPierce,
    Slicing,
    Wyvern,
    Tranq,
    Demon,
    Armor,
}