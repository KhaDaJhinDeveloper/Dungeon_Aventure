using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //---------------Room Mounting Point--------------------
    public enum Direction { Up, Down, Left, Right }
    public Exits[] exits;
    [HideInInspector] public Room originalPrefab;
    #region SpawnEnemy;
    public List<SpawnPoint> availablePoints = new List<SpawnPoint>();
    #endregion
    #region SpawnBonFire;
    public List<SpawnBonFire> bonFirePoints = new List<SpawnBonFire>();
    #endregion
    #region SpawnBox;
    public List<SpawnBoxPoint> boxPoint = new List<SpawnBoxPoint>();
    #endregion
    [System.Serializable]
    public class Exits
    {
        public Transform exitPoint; 
        public Direction exitDirections; 
        public bool isUsed;
    }
    public List<SpawnPoint> GetSpawnPoint()
    {
        List<SpawnPoint> pointUnused = new List<SpawnPoint>();
        if(availablePoints.Count == 0) return null;
        foreach (SpawnPoint point in availablePoints)
        {
            if(!point.isUsed) pointUnused.Add(point);
        }
        if (pointUnused.Count == 0) return null;
        return pointUnused;
    }
}