using PrimaryParameter.SG;

namespace CVM;

public partial class CVM<T>([Field]int memory, [Field(Type = typeof(Random), AssignFormat = "{0} ?? Random.Shared")]Random? rng = default)
{
    private int _round = 0;
    private LinkedList<T> _array = new();

    private bool IsFull => _array.Count >= _memory;

    public int Count => _array.Count * (int)Math.Pow(2, _round);

    public void Process(T data)
    {
        if (!_array.Contains(data))
            if (_rng.NextBool(_round))
            _array.AddLast(data);
        if (IsFull)
            Clean();
    }

    private void Clean()
    {
        var current = _array.Last;
        while (current is not null)
        {
            if (_rng.NextBool())
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
