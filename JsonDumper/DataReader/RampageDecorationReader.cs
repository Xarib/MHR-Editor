using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class RampageDecorationReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReDataFile
            .Read(PathHelper.CHUNK_PATH +
                  @"\natives\STM\data\Define\Player\Equip\HyakuryuDeco\HyakuryuDecoBaseData.user.2")
            .rsz
            .objectData
            .OfType<Snow_data_HyakuryuDecoBaseUserData_Param>()
            .Select(decoration => new RampageDecoration()
            {
                Id = decoration.Id,
                SlotSize = (uint)decoration.SlotType,
                Name = DataHelper.RAMPAGE_DECORATION_NAME_LOOKUP[Global.LangIndex.eng][decoration.Id],
                Rarity = ReaderHelper.ConvertRarity(decoration.Rare),
                RampageSkill = decoration.HyakuryuSkillId,
            });
    }
}