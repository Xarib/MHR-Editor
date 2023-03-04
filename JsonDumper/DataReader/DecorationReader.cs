using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class DecorationReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReDataFile
            .Read(PathHelper.CHUNK_PATH +
                  @"\natives\STM\data\Define\Player\Equip\Decorations\DecorationsBaseData.user.2")
            .rsz
            .objectData
            .OfType<Snow_data_DecorationsBaseUserData_Param>()
            .Where(decoration => decoration.BasePrice > 0)
            .Select(decoration => new Decoration()
            {
                Id = decoration.Id,
                Name = DataHelper.DECORATION_NAME_LOOKUP[Global.LangIndex.eng][decoration.Id],
                Rarity = ReaderHelper.ConvertRarity(decoration.Rare),
                SlotSize = (uint)decoration.DecorationLv,
                Skills = ReaderHelper.ConvertSkill(decoration.SkillIdList, decoration.SkillLvList).ToList(),
            });
    }
}