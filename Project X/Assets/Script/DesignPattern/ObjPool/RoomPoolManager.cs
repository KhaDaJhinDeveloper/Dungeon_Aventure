using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPoolManager : MonoBehaviour
{
    public static RoomPoolManager RoomPool_Instance;
    [System.Serializable]
    public class Pool
    {
        public Room prefab;
        public int size;
        [HideInInspector] public Queue<Room> objects = new Queue<Room>();
    }
    public List<Pool>  roomPools = new List<Pool>();
    private  Dictionary<Room, Pool> prefabPoolMap = new Dictionary<Room, Pool>();
    void Awake()
    {
        RoomPool_Instance = this;
        foreach(Pool pool in roomPools)
        {
            prefabPoolMap[pool.prefab] = pool;
            for(int i = 0; i < pool.size; i++)
            {
                Room obj = Instantiate(pool.prefab,transform);
                obj.originalPrefab = pool.prefab; 
                obj.gameObject.SetActive(false);
                pool.objects.Enqueue(obj);
            }    
        }    
    }
    public Room GetRoom(Room prefab)
    {
        Pool pool;
        if (!prefabPoolMap.TryGetValue(prefab, out pool))
        {
            pool = new Pool { prefab = prefab, size = 0 };
            prefabPoolMap[prefab] = pool;
            roomPools.Add(pool);
        }
        return prefab;
    }
}
