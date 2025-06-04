using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //---------------Room Mounting Point--------------------
    public enum Direction { Up, Down, Left, Right }  
    [System.Serializable]
    public class Exits
    {
        public Transform exitPoint; 
        public Direction exitDirections; 
        [HideInInspector] public bool isUsed;
    }
    public Exits[] exits;
    [HideInInspector] public Room originalPrefab ;
    //---------------Point Spawn Enemy--------------------
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        [HideInInspector]public bool isUsed;
    }
    public List<SpawnPoint> availablePoints = new List<SpawnPoint>();
    public List<SpawnPoint> GetSpawnPoint()
    {
        List<SpawnPoint> pointUnused = new List<SpawnPoint>();
        if(availablePoints.Count == 0) return null;
        foreach (SpawnPoint point in availablePoints)
        {
            if(!point.isUsed) pointUnused.Add(point);
        }
        if (pointUnused.Count == 0) return null;
        //SpawnPoint randomPointSpawn = pointUnused[Random.Range(0, pointUnused.Count)];
        return pointUnused;
    }
}