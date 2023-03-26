using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class RampageDecoration : IGameData, IRarity, IName, ISlotSize
{
    [JsonIgnore]
    public long Id { get; init; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public uint SlotSize { get; set; }
    public int RampageSkill { get; set; }
}