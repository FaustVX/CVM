using PrimaryParameter.SG;

namespace CVM;

public partial class CVM<T>([Field]int memory)
{
    public Random Rng { private get; init; } = Random.Shared;
    private int _round = 0;
    private LinkedList<T> _array = new();

    private bool IsFull => _array.Count >= _memory;

    public int Count => _array.Count * (int)Math.Pow(2, _round);

    public void Process(T data)
    {
        if (!_array.Contains(data))
            if (GenerateBool(_round, Rng))
            _array.AddLast(data);
        if (IsFull)
            Clean();

        static bool GenerateBool(int round, Random rng)
        => round switch
        {
            <= 0 => true,
            1 => rng.NextBool(),
            _ => rng.NextBool() && GenerateBool(round - 1, rng),
        };
    }

    private void Clean()
    {
        var current = _array.Last;
        while (current is not null)
        {
            if (Rng.NextBool())
            {
                (current, var next) = (current.Previous, current!);
                _array.Remove(next);
                continue;
            }
            current = current.Previous;
        }
        _round++;
    }
}
