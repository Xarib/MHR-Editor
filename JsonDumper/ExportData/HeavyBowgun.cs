using JsonDumper.DataReader;
using JsonDumper.ExportData.Traits;
using MHR_Editor.Models.Enums;

namespace JsonDumper.ExportData;

public class HeavyBowgun : IGameData, IAttack, ICriticalRate, IDefenseBonus, IRampageSlots, ISlots, IRarity, IName, IMagazine, IGun
{
    public long Id { get; init; }
    public int Attack { get; set; }
    public int CriticalRate { get; set; }
    public int DefenseBonus { get; set; }
    public IList<int> RampageSlots { get; set; }
    public IList<int> Slots { get; set; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public IList<Magazine> Magazines { get; set; }
    public Snow_data_GameItemEnum_Fluctuation Fluctuation { get; set; }
    public Snow_data_GameItemEnum_Reload Reload { get; set; }
    public Snow_data_GameItemEnum_Recoil Recoil { get; set; }
    public Snow_data_GameItemEnum_KakusanType ClusterBombType { get; set; }
    public Snow_data_HeavyBowgunWeaponData_UniqueBulletType SpecialAmmo { get; set; }
}