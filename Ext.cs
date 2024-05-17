
namespace CVM;

public static class Ext
{
    private static (int, int) _currentPower = (0, 1);

    public static bool NextBool(this Random rng)
    => rng.Next(2) == 0;

    public static bool NextBool(this Random rng, int power)
    => rng.Next(_currentPower.Item1 == power ? _currentPower.Item2 : (_currentPower = (power, (int)Math.Pow(2, power))).Item2) == 0;
}
