using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class PetalaceReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReDataFile
            .Read(PathHelper.CHUNK_PATH +
                  @"\natives\STM\data\System\ContentsIdSystem\LvBuffCage\Normal\NormalLvBuffCageBaseData.user.2")
            .rsz
            .objectData
            .OfType<Snow_data_NormalLvBuffCageBaseUserData_Param>()
            .Where(petalace => petalace.Name.ToUpperInvariant() != "unknown".ToUpperInvariant())
            .Select(petalace => new Petalace()
            {
                Id = petalace.Id,
                Name = DataHelper.PETALACE_NAME_LOOKUP[Global.LangIndex.eng][petalace.Id],
                Rarity = ReaderHelper.ConvertRarity(petalace.Rarity),
            });
    }
}