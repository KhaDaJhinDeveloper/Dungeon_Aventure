using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : SpawnBase
{
    RoomManager roomManager;
    protected override void Start()
    {
        this.roomManager = GetComponent<RoomManager>();    
        base.Start();
           
    }
    protected override void Spawn()
    {      
        foreach(Room room in this.roomManager.placedRooms)
        {
            List<Room.SpawnPoint> pointList = room.GetSpawnPoint();
            { 
                if(pointList == null) continue;
                foreach(Room.SpawnPoint pointSpawn in pointList)
                {
                    string randomKey = prefab[Random.Range(0, prefab.Count)].name;
                    GameObject enemy =  ObjectPooling.ObjectPooling_Instance.GetPool(randomKey);
                    enemy.transform.position = pointSpawn.point.transform.position;
                    pointSpawn.isUsed = true;
                }
            }          
        }
    }    
}
