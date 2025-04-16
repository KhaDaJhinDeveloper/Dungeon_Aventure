using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    public static ObjectPooling ObjectPooling_Instance {  get; private set; }
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    protected override void Awake()
    {
        base.Awake();
        ObjectPooling_Instance = this;
    }
    public void CreatePool(string key, GameObject prefab,int poolSize)
    {
        if(!poolDictionary.ContainsKey(key))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for(int i = 0; i < poolSize; i++ )
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDictionary.Add(key, queue);
        }
    }   
    public GameObject GetPool(string key)
    {
        if(!poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }    
        return null;
    }
    public void ReturnToPool(string key, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }  
        prefab.SetActive(false);
        poolDictionary[key].Enqueue(prefab);
    }
}
