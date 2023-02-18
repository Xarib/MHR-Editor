namespace JsonDumper.DataReader;

public static class ReaderHelper
{
    public static string GetWeaponPath(string weapon)
        => $@"\natives\STM\data\Define\Player\Weapon\{weapon}\{weapon}BaseData.user.2";
}