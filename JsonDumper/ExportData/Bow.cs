using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class Bow : IGameData, IAttack, ICriticalRate, IDefenseBonus, IRampageSlots, ISlots, IWeaponElement, IRarity, IName
{
    [JsonIgnore]
    public long Id { get; init; }
    public int Attack { get; set; }
    public int CriticalRate { get; set; }
    public int DefenseBonus { get; set; }
    public IList<int> RampageSlots { get; set; }
    public IList<int> Slots { get; set; }
    public WeaponElement? WeaponElement { get; set; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public IList<Coating> Coatings { get; set; }
    public int MaxChargeLevel { get; set; }
    public IList<Charge> Charges { get; set; }
}

public class Coating
{
    public int Type { get; set; }
    public bool Upgraded { get; set; }
}

public class Charge
{
    public ChargeType Type { get; set; }
    public int Level { get; set; }
}

public enum ChargeType
{
    Spread = 1,
    Pirce = 2,
    Rapid = 3,
}