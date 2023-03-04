using JsonDumper.ExportData.Traits;

namespace JsonDumper.ExportData;

public class Decoration : IGameData, IRarity, IName, ISkill, ISlotSize
{
    public long Id { get; init; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public uint SlotSize { get; set; }
    public IList<Skill> Skills { get; set; }
}