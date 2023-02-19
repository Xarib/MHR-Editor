using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;
using MHR_Editor.Models.Enums;

namespace JsonDumper.ExportData;

public class GreatSword : IGameData, IAttack, ICriticalRate, IDefenseBonus, IRampageSlots, ISharpness, ISlots, IWeaponElement, IRarity, IName
{
    [JsonIgnore]
    public long Id { get; init; }
    public int Attack { get; set; }
    public int CriticalRate { get; set; }
    public int DefenseBonus { get; set; }
    public IList<int> RampageSlots { get; set; }
    public IList<int> Sharpness { get; set; }
    public IList<int> Handicraft { get; set; }
    public IList<int> Slots { get; set; }
    public WeaponElement? WeaponElement { get; set; }
    public Snow_data_DataDef_RareTypes Rarity { get; set; }
    public string Name { get; set; }
}