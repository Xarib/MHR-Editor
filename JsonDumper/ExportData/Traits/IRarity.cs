using MHR_Editor.Models.Enums;

namespace JsonDumper.ExportData.Traits;

public interface IRarity
{
    public Snow_data_DataDef_RareTypes Rarity { get; set; }
}