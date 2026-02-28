using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : SpawnBase
{
    private RoomManager roomManager;
    protected override void Start()
    {
        this.roomManager = GetComponent<RoomManager>();
        base.Start();
    }
    protected override void Spawn()
    {
        if (EnemiesDataManager.Instance.HasData())
            return;  
        else
            SpawnNewEnemies();
    }
    private void SpawnNewEnemies()
    {
        DebugLogger.Log("Spawn");
        if(this.roomManager != null)
        {
            foreach (Room room in this.roomManager.placedRooms)
            {
                List<SpawnPoint> pointList = room.GetSpawnPoint();
                {
                    if (pointList == null) continue;
                    foreach (SpawnPoint pointSpawn in pointList)
                    {
                        string randomKey = prefab[Random.Range(0, prefab.Count)].name;
                        GameObject enemy = ObjectPooling.ObjectPooling_Instance.GetPool(randomKey);
                        string uniqued = System.Guid.NewGuid().ToString();
                        EnemyIDTracker tracker = enemy.GetComponent<EnemyIDTracker>();
                        tracker.uniqueID = uniqued;
                        enemy.transform.position = pointSpawn.point.transform.position;
                        pointSpawn.isUsed = true;
                    }
                }
            }
        }
    }
}
