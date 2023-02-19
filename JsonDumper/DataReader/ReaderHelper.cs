using System.Collections;
using System.Collections.ObjectModel;
using JsonDumper.ExportData;
using MHR_Editor.Common;
using MHR_Editor.Common.Models;
using MHR_Editor.Common.Models.List_Wrappers;
using MHR_Editor.Models.Enums;

namespace JsonDumper.DataReader;

public static class ReaderHelper
{
    public static string GetWeaponPath(string weapon)
        => $@"\natives\STM\data\Define\Player\Weapon\{weapon}\{weapon}BaseData.user.2";

    public static IEnumerable<T> GetWeaponData<T>(string weapon) where T : RszObject
    {
        var data = ReDataFile.Read(PathHelper.CHUNK_PATH + GetWeaponPath(weapon)).rsz.objectData;

        foreach (var anyInstance in data)
        {
            if (anyInstance is not T instance)
                continue;

            yield return instance;
        }
    }

    public static int ConvertRarity(Snow_data_DataDef_RareTypes  rarity)
        => ((int)rarity) + 1;

    public static WeaponElement? ConvertWeaponElement(Snow_equip_PlWeaponElementTypes element, int value)
    {
        if (element == Snow_equip_PlWeaponElementTypes.None)
            return null;

        return new WeaponElement(element, value);
    }

    public static IEnumerable<int> ConvertRampageSlots(ObservableCollection<GenericWrapper<uint>> slots)
    {
        foreach (var slot in ConvertSlotSize(3, slots))
            yield return slot;
        
        foreach (var slot in ConvertSlotSize(2, slots))
            yield return slot;
        
        foreach (var slot in ConvertSlotSize(1, slots))
            yield return slot;
    }
    
    public static IEnumerable<int> ConvertSlots(ObservableCollection<GenericWrapper<uint>> slots)
    {
        foreach (var slot in ConvertSlotSize(3, slots))
            yield return slot;
        
        foreach (var slot in ConvertSlotSize(2, slots))
            yield return slot;
        
        foreach (var slot in ConvertSlotSize(1, slots))
            yield return slot;
    }

    public static IEnumerable<int> ConvertSharpness(ObservableCollection<GenericWrapper<int>> slots)
    {
        yield return slots[0].Value;
        yield return slots[1].Value;
        yield return slots[2].Value;
        yield return slots[3].Value;
        yield return slots[4].Value;
        yield return slots[5].Value;
        yield return slots[6].Value;
    }

    public static IEnumerable<int> ConvertHandicraft(ObservableCollection<GenericWrapper<int>> slots)
    {
        yield return slots[0].Value;
        yield return slots[1].Value;
        yield return slots[2].Value;
        yield return slots[3].Value;
    }

    private static IEnumerable<int> ConvertSlotSize(int slotSize, ObservableCollection<GenericWrapper<uint>> slots)
    {
        var index = slotSize - 1;
        
        for (uint i = 0; i < slots[index].Value; i++)
            yield return slotSize;
    }
}