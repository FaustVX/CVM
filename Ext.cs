
namespace CVM;

public static class Ext
{
    public static bool NextBool(this Random rng)
    => rng.Next(2) == 0;

    public static bool NextBool(this Random rng, int power)
    => rng.Next((int)Math.Pow(2, power)) == 0;
}
