using System.Collections;

namespace Hainz.Core.Collections;

public sealed class BidirectionalMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> where T1 : notnull where T2 : notnull
{
    private readonly Dictionary<T1, T2> _forward;
    private readonly Dictionary<T2, T1> _reverse;

    public ReadonlyDictionaryIndexer<T1, T2> Forward => new(_forward);
    public ReadonlyDictionaryIndexer<T2, T1> Reverse => new(_reverse);

    public BidirectionalMap() 
    {
        _forward = new Dictionary<T1, T2>();
        _reverse = new Dictionary<T2, T1>();
    }

    public void Add(T1 elem1, T2 elem2) 
    {
        if (!_forward.TryAdd(elem1, elem2))
            throw new ArgumentException("Element already in map", nameof(elem1));
        if (!_reverse.TryAdd(elem2, elem1)) 
        {
            _forward.Remove(elem1);
            throw new ArgumentException("Element already in map", nameof(elem2));
        }
    }

    public bool Remove(T1 elem1, T2 elem2) 
    {
        if (_forward.Remove(elem1)) 
        {
            _reverse.Remove(elem2);
            return true;
        }

        return false;
    }

    public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}