
namespace CVM;

public static class Ext
{
    public static bool NextBool(this Random rng)
    => rng.Next(2) == 0;
}
