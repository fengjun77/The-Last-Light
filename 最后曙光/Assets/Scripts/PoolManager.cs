using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    //保存所有池子，key为预制体
    private Dictionary<MonoBehaviour, object> _poolDict = new Dictionary<MonoBehaviour, object>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>初始化对应预制体的对象池，外部初始化调用</summary>
    public void CreatePool<T>(T prefab, Transform parent, int initCount = 10) where T : MonoBehaviour
    {
        if (_poolDict.ContainsKey(prefab)) return;
        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, initCount);
        _poolDict.Add(prefab, pool);
    }

    /// <summary>取出物体</summary>
    public T GetItem<T>(T prefab) where T : MonoBehaviour
    {
        if (_poolDict.TryGetValue(prefab, out var pool))
        {
            return ((ObjectPool<T>)pool).Get();
        }
        Debug.LogError($"不存在{prefab.name}的对象池，请先CreatePool");
        return null;
    }

    /// <summary>归还物体，全局统一入口，这就是你想要的：归还逻辑集中在这里</summary>
    public void ReturnItem<T>(T prefab, T instance) where T : MonoBehaviour
    {
        if (_poolDict.TryGetValue(prefab, out var pool))
        {
            ((ObjectPool<T>)pool).Return(instance);
        }
    }
}
