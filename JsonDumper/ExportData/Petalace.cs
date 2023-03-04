using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class Petalace : IGameData, IRarity, IName
{
    [JsonIgnore]
    public long Id { get; init; }
    public int Rarity { get; set; }
    public string Name { get; set; }
}