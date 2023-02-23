using System.Collections.ObjectModel;
using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models;
using MHR_Editor.Common.Models.List_Wrappers;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class ArmorReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    { 
        return ReDataFile
            .Read(PathHelper.CHUNK_PATH + @"\natives\STM\data\Define\Player\Armor\ArmorBaseData.user.2")
            .rsz
            .objectData
            .OfType<Snow_data_ArmorBaseUserData_Param>()
            .Where(armor => armor.IsValid)
            .Select(armor => new Armor()
            {
                Id = armor.Id,
                Slots = ReaderHelper.ConvertSlots(armor.DecorationsNumList).ToList(),
                Name = DataHelper.ARMOR_NAME_LOOKUP[Global.LangIndex.eng][armor.Id],
                Rarity = ReaderHelper.ConvertRarity(armor.Rare),
                Defense = armor.DefVal,
                FireResistance = armor.FireRegVal,
                WaterResistance = armor.WaterRegVal,
                IceResistance = armor.IceRegVal,
                ThunderResistance = armor.ThunderRegVal,
                DragonResistance = armor.DragonRegVal,
                Skills = ConvertSkill(armor.SkillList, armor.SkillLvList).ToList(),
            });
    }

    private static IEnumerable<Skill> ConvertSkill(ObservableCollection<DataSourceWrapper<byte>> skillId,
        ObservableCollection<GenericWrapper<int>> skillLevel)
    {
        for (var i = 0; i < skillId.Count; i++)
        {
            var id = skillId[i].Value;
            
            if (id == 0)
                continue;

            yield return new Skill()
            {
                Id = id,
                Level = skillLevel[i].Value,
            };
        }
    }
}