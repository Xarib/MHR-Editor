using JsonDumper.GameData;

namespace JsonDumper.DataReader;

public class GreatSwordReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        yield return new GreatSword(123);
    }
}