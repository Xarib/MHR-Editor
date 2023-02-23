using System.Text.Json.Serialization;
using JsonDumper.ExportData.Traits;
using MHR_Editor.Models.Enums;

namespace JsonDumper.ExportData;

public class Insect : IGameData, IRarity, IName
{
    [JsonIgnore]
    public long Id { get; init; }
    public int Rarity { get; set; }
    public string Name { get; set; }
    public Snow_data_InsectData_AtkTypes InsectAtkType { get; set; }
    public Snow_data_InsectData_ButtleTypes InsectButtleType { get; set; }
    public IList<int> AttackPerLevel { get; set; }
    public IList<int> SpeedPerLevel { get; set; }
    public IList<int> HealPerLevel { get; set; }
    public Snow_data_InsectData_InsectSkillId InsectSkillId { get; set; }
    public Snow_data_InsectData_DustTypes DustType { get; set; }
}