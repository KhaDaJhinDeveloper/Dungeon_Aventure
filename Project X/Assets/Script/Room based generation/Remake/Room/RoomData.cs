using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomData : MonoBehaviour
{
    public ExitsRoom[] exits;
    [Header("SpawnPoint")]
    public Transform bonFirePoint;
    public Transform[] boxPoints;
    public Transform[] enemiesPoint;
}
[System.Serializable]
public class ExitsRoom
{
    public Transform exitPoint;
    public Direction exitDirections;
    public bool isUsed;
}
