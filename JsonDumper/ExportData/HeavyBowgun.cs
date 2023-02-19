using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class HeavyBowgun : IGameData, IAttack, ICriticalRate, IDefenseBonus, IRampageSlots, ISlots, IRarity, IName
{
    public long Id { get; init; }
    public int Attack { get; set; }
    public int CriticalRate { get; set; }
    public int DefenseBonus { get; set; }
    public IList<int> RampageSlots { get; set; }
    public IList<int> Slots { get; set; }
    public int Rarity { get; set; }
    public string Name { get; set; }
}