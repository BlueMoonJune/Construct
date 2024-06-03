using System.Collections.Generic;

namespace Construct.Util;

public class ListDictionary<K, V>
{
    public Dictionary<K, List<V>> _dict = new();
    public List<V> this[K k]
    {
        get => _dict[k];
        set => _dict[k] = value;
    }
    public V this[K k, int i]
    {
        get => _dict[k][i];
    }
    public void Add(K k, V v)
    {
        if (!_dict.ContainsKey(k))
        {
            _dict[k] = new List<V>();
        }
        _dict[k].Add(v);
    }
    public List<V> Get(K k) => this[k];
    public V Get(K k, int i) => this[k, i];

    public List<V> GetOrEmpty(K k)
    {
        if (!_dict.ContainsKey(k))
        {
            return new();
        }
        return _dict[k];
    }

    public void Remove(K k, V v)
    {
        _dict[k].Remove(v);
        if (_dict[k].Count == 0)
        {
            _dict.Remove(k);
        }
    }
}
