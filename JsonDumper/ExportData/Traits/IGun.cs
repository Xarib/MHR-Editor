using MHR_Editor.Models.Enums;

namespace JsonDumper.ExportData.Traits;

public interface IGun
{
    public Snow_data_GameItemEnum_Fluctuation Fluctuation { get; set; }
    public Snow_data_GameItemEnum_Reload Reload { get; set; }
    public Snow_data_GameItemEnum_Recoil Recoil { get; set; }
    public Snow_data_GameItemEnum_KakusanType ClusterBombType { get; set; }
}