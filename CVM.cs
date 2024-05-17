using PrimaryParameter.SG;

namespace CVM;

public partial class CVM<T>([Field]int memory)
{
    public Random Rng { get; init; } = Random.Shared;
    private int _round = 0;
    private LinkedList<T> _array = new();

    private bool IsFull => _array.Count >= _memory;

    public void Process(T data)
    {
        if (!_array.Contains(data))
            _array.AddLast(data);
        if (IsFull)
            Clean();
    }

    private void Clean()
    {
        if (_round == 0)
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
        }
        _round++;
    }
}
