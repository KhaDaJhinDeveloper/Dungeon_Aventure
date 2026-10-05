using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OpenExit
{
    public Vector2Int fromGridPos;
    public Direction exitDirection;
}
public class PlacedRoomNode
{
    public RoomData prefab;
    public Vector2Int gridPos;
    public PlacedRoomNode(RoomData prefab, Vector2Int gridPos)
    {
        this.prefab = prefab;
        this.gridPos = gridPos;
    }
    public bool HasExitDirection(Direction Dir)
    {
        foreach(var exit in prefab.exits)
        {
            if(exit.exitDirections == Dir)
                return true;
        }
        return false;
    }
}
