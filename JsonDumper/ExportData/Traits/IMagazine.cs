using JsonDumper.DataReader;

namespace JsonDumper.ExportData.Traits;

public interface IMagazine
{
    public IList<Magazine> Magazines { get; set; }
}