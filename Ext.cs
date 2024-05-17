
namespace CVM;

public static class Ext
{
    private static Dictionary<int, int> _powers = [];
    public static bool NextBool(this Random rng)
    => rng.Next(2) == 0;

    public static bool NextBool(this Random rng, int power)
    => rng.Next(_powers.TryGetValue(power, out var value) ? value : (_powers[power] = (int)Math.Pow(2, power))) == 0;
}
