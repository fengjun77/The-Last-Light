using System.Collections.Generic;
using UnityEngine;

/// <summary>通用对象池，独立工具类，完全可复用</summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _poolQueue = new Queue<T>();

    /// <summary>初始化对象池</summary>
    /// <param name="prefab">预制体</param>
    /// <param name="parent">父物体</param>
    /// <param name="initCount">预先实例化数量</param>
    public ObjectPool(T prefab, Transform parent, int initCount = 10)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initCount; i++)
        {
            T obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }

    /// <summary>取出对象</summary>
    public T Get()
    {
        T item;
        if (_poolQueue.Count > 0)
        {
            item = _poolQueue.Dequeue();
        }
        else
        {
            //池子里用完再动态创建
            item = Object.Instantiate(_prefab, _parent);
        }
        item.gameObject.SetActive(true);
        return item;
    }

    /// <summary>归还对象</summary>
    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        _poolQueue.Enqueue(item);
    }
}
