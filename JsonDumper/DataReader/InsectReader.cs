using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Models.Structs;

namespace JsonDumper.DataReader;

public class InsectReader : IDataReader
{
    public IEnumerable<IGameData> GetData()
    {
        return ReaderHelper
            .GetWeaponData<Snow_equip_InsectBaseUserData_Param>(nameof(Insect))
            .Select(i => new Insect()
            {
                Id = i.Id,
                Rarity = ReaderHelper.ConvertRarity(i.RareType),
                Name = DataHelper.WEAPON_NAME_LOOKUP[Global.LangIndex.eng][i.Id],
                AttackPerLevel = i.InsectAtkTableIndexList.
                    Select(wr => (int)wr.Value)
                    .ToList(),
                SpeedPerLevel = i.InsectAtkTableIndexList
                    .Select(wr => (int)wr.Value)
                    .ToList(),
                HealPerLevel = i.InsectAtkTableIndexList
                    .Select(wr => (int)wr.Value)
                    .ToList(),
                DustType = i.DustType,
                InsectAtkType = i.InsectAtkType,
                InsectButtleType = i.InsectButtleType,
                InsectSkillId = i.InsectSkillId,
            });
    }
}