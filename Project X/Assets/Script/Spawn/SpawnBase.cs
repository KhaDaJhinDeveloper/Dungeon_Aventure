using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBase : MonoBehaviour
{
    [SerializeField] protected List<GameObject> prefab = new List<GameObject>();
    [SerializeField] protected int poolSize;
    protected virtual void Start()
    {
        CreatePool();
        Spawn();
    }
    protected virtual void CreatePool()
    {
        if (prefab.Count < 0) DebugLogger.Log("prefab null");
        for (int i = 0; i < this.prefab.Count; i++)
        {
            ObjectPooling.ObjectPooling_Instance.CreatePool(prefab[i].name, prefab[i], poolSize);
        }
    }
    protected virtual void Spawn()
    {

    }    
}
