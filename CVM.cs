using PrimaryParameter.SG;

namespace CVM;

public partial class CVM<T>([Field]int memory, [Field(Type = typeof(Random), AssignFormat = "{0} ?? Random.Shared")]Random? rng = default)
{
    private int _round = 0;
    private LinkedList<T> _storage = new();

    private bool IsFull => _storage.Count >= _memory;

    public int Count => _storage.Count * (int)Math.Pow(2, _round);

    public void Process(T data)
    {
        if (!_storage.Contains(data))
            if (_rng.NextBool(_round))
            _storage.AddLast(data);
        if (IsFull)
            Clean();
    }

    private void Clean()
    {
        var current = _storage.Last;
        while (current is not null)
            if (_rng.NextBool())
            {
                (current, var next) = (current.Previous, current!);
                _storage.Remove(next);
            }
            else
                current = current.Previous;
        _round++;
    }
}
