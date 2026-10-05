using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBase : MonoBehaviour
{
    [SerializeField] protected PoolEntry[] poolEntries;
    protected virtual void Start()
    {
        CreatePool();
        Spawn();
    }
    protected virtual void CreatePool()
    {
        if (this.poolEntries.Length < 0) DebugLogger.Log("prefab null");
        foreach (PoolEntry obj in this.poolEntries)
        {
            int index = 0; 
            while(index < obj.poolSize)
            {
                ObjectPooling.ObjectPooling_Instance.CreatePool(obj.key, obj.prefab, obj.poolSize);
                index++;
            }    
        }    
    }
    protected virtual void Spawn()
    {

    }    
}
[System.Serializable]
public struct PoolEntry
{
    public GameObject prefab;
    public int poolSize;
    public KeyPool key;
}
