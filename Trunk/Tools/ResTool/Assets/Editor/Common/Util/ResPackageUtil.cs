public enum ResPackage
{
    Base,
    Eliminate,
    Mahjong,
    Coin,
    Animal,
}

public class ResPackageUtil
{
    public static string GetPackageName(ResPackage resPackage)
    {
        switch (resPackage)
        {
            case ResPackage.Base:
                return "base";
            case ResPackage.Eliminate:
                return "eliminate";
            case ResPackage.Mahjong:
                return "mahjonghul";
            case ResPackage.Coin:
                return "coin";
            case ResPackage.Animal:
                return "animal";
            default:
                return "";
        }
    }
}
