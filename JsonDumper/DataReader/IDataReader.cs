using JsonDumper.ExportData;

namespace JsonDumper.DataReader;

public interface IDataReader
{
    public IEnumerable<IGameData> GetData();
}