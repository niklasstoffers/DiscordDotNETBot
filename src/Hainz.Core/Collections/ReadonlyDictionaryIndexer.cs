namespace Hainz.Core.Collections;

public sealed class ReadonlyDictionaryIndexer<T1, T2> where T1 : notnull where T2: notnull
{
    private readonly Dictionary<T1, T2> _dictionary;

    public ReadonlyDictionaryIndexer(Dictionary<T1, T2> dictionary)
    {
        _dictionary = dictionary;
    }

    public T2 this[T1 key] 
    {
        get => _dictionary[key];
    }

    public T2? GetValueOrDefault(T1 key) => _dictionary.GetValueOrDefault(key);
}