using JsonDumper.GameData;

namespace JsonDumper.DataReader;

public interface IDataReader
{
    public IEnumerable<IGameData> GetData();
}