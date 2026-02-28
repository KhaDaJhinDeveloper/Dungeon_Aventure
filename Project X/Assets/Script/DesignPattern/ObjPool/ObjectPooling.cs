using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooling : Singleton<ObjectPooling>
{
    public static ObjectPooling ObjectPooling_Instance {  get; private set; }
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    protected override void Awake()
    {
        base.Awake();
        ObjectPooling_Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void CreatePool(string key, GameObject prefab,int poolSize)
    {
        string keyClean = CleanKey(key);
        if(!poolDictionary.ContainsKey(keyClean))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for(int i = 0; i < poolSize; i++ )
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDictionary.Add(keyClean, queue);
            prefabDictionary[keyClean] = prefab;
        }
    }   
    public GameObject GetPool(string key)
    {
        string cleankey = CleanKey(key);
        if(poolDictionary.ContainsKey(cleankey))
        {            
            if(poolDictionary[cleankey].Count > 0)
            {
                GameObject obj = poolDictionary[cleankey].Dequeue();
                if (obj != null)
                { 
                    obj.SetActive(true);
                    return obj;
                }
            }    
            else if (prefabDictionary.ContainsKey(cleankey))
            {
                GameObject newObj = Instantiate(prefabDictionary[cleankey]);
                newObj.SetActive(true);
                return newObj;
            }
        }    
        return null;
    }
    public void ReturnToPool(string key, GameObject prefab)
    {
        string cleankey = CleanKey(key);
        if (!poolDictionary.ContainsKey(cleankey))
        {
            poolDictionary[cleankey] = new Queue<GameObject>();
        }  
        prefab.SetActive(false);
        poolDictionary[cleankey].Enqueue(prefab);
    }
    private string CleanKey(string rawKey)
    {
        return rawKey.Replace("(Clone)", "").Trim();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PoolClear();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void PoolClear()
    {
        this.poolDictionary.Clear();
    }    
}
