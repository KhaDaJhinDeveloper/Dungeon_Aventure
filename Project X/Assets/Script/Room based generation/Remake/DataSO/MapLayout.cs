using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Map/LayoutMap")]
public class MapLayout : ScriptableObject
{
    [Tooltip("The starting room will always be the first element")]
    public RoomData[] roomBody;
    public RoomData[] roomExit;
    public RoomData[] wall;
    [Header("EnemySpawnData")]
    public List<EnemySpawnData> enemyDataSpawnList;
    [Header("Interact Object Data")]
    public int boxQty;
    public int bonFireQty;
}
[System.Serializable]
public struct EnemySpawnData
{
    public KeyPool enemyPool;
    public int unlockLevel;
    public int weight;
}