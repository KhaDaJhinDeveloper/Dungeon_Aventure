using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooling : Singleton<ObjectPooling>
{
    public static ObjectPooling ObjectPooling_Instance { get; private set; }
    private Dictionary<KeyPool, Queue<GameObject>> poolDictionary = new Dictionary<KeyPool, Queue<GameObject>>();
    private Dictionary<KeyPool, GameObject> prefabDictionary = new Dictionary<KeyPool, GameObject>();
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        ObjectPooling_Instance = this;
    }
    public void CreatePool(KeyPool key, GameObject prefab, int poolSize, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError($"Cannot create pool for {key}: prefab null.", this);
            return;
        }

        prefabDictionary[key] = prefab;
        if (!poolDictionary.ContainsKey(key))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.SetParent(parent);
                queue.Enqueue(obj);
            }
            poolDictionary.Add(key, queue);
        }
    }
    public GameObject GetPool(KeyPool key, Transform parent = null)
    {
        if (poolDictionary.ContainsKey(key))
        {
            while (poolDictionary[key].Count > 0)
            {
                GameObject obj = poolDictionary[key].Dequeue();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(parent);
                    return obj;
                }
            }
        }

        if (prefabDictionary.TryGetValue(key, out GameObject prefab) && prefab != null)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(true);
            newObj.transform.SetParent(parent);
            return newObj;
        }

        DebugLogger.Log($"Pool {key} has not been created or its prefab is missing.", this);
        return null;
    }
    public void ReturnToPool(KeyPool key, GameObject prefab)
    {
        if (prefab == null) return;
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }
        prefab.SetActive(false);
        if (poolDictionary[key].Contains(prefab)) return;
        poolDictionary[key].Enqueue(prefab);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (ObjectPooling_Instance == this)
            ObjectPooling_Instance = null;
    }
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        PoolClear();
    }
    public void PoolClear()
    {
        this.poolDictionary.Clear();
    }
}
