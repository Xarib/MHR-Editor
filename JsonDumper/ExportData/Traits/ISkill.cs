namespace JsonDumper.ExportData.Traits;

public interface ISkill
{
    public IList<Skill> Skills { get; set; }
}
public class Skill
{
    public int Id { get; set; }
    public int Level { get; set; }
}
