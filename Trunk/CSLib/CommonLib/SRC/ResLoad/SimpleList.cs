using System.Collections.Generic;

public interface UniqueKeyData<T>
{
    T mUniqueKey { get; }
}

/// <summary>
/// 简单的列表类，集合字典和列表的功能
/// </summary>
/// <typeparam name="K">mUniqueKey</typeparam>
/// <typeparam name="V">UniqueKeyData</typeparam>
public class SimpleList<K, V> where V : UniqueKeyData<K>
{
    private readonly Dictionary<K, int> m_Dict = new Dictionary<K, int>();
    private readonly List<V> m_List = new List<V>();

    public void Add(V value)
    {
        m_Dict.Add(value.mUniqueKey, m_List.Count);
        m_List.Add(value);
    }

    public bool ContainsKey(K key)
    {
        return m_Dict.ContainsKey(key);
    }

    public V GetByKey(K key)
    {
        return m_List[m_Dict[key]];
    }

    public V GetByIndex(int index)
    {
        return m_List[index];
    }

    public void Remove(K key)
    {
        int index = m_Dict[key];
        m_Dict.Remove(key);
        m_List.RemoveAt(index);
        Adjust(index);
    }

    public void RemoveAt(int index)
    {
        V value = m_List[index];
        m_List.RemoveAt(index);
        m_Dict.Remove(value.mUniqueKey);
        Adjust(index);
    }

    private void Adjust(int index)
    {
        for (int i = index; i < m_List.Count; i++)
            m_Dict[m_List[i].mUniqueKey] = i;
    }

    public int Count
    {
        get { return m_List.Count; }
    }

    public void Clear()
    {
        m_List.Clear();
        m_Dict.Clear();
    }
}