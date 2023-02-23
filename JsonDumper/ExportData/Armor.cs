using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class Armor : IGameData, ISlots, IRarity, IName
{
    [JsonIgnore]
    public long Id { get; init; }
    public IList<int> Slots { get; set; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public int Defense { get; set; }
    public int FireResistance { get; set; }
    public int WaterResistance { get; set; }
    public int IceResistance { get; set; }
    public int ThunderResistance { get; set; }
    public int DragonResistance { get; set; }
    public IList<Skill> Skills { get; set; }
}

public class Skill
{
    public int Id { get; set; }
    public int Level { get; set; }
}