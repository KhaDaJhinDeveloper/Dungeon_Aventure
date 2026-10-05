using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomDataNode
{
    public string roomPrefabName;
    public int x;
    public int y;
    public RoomDataNode(string name, int x, int y)
    {
        this.roomPrefabName = name;
        this.x = x;
        this.y = y;
    }
}
[System.Serializable]
public class SaveRoomData
{
    public string mapID;
    public List<RoomDataNode> listRoonNode = new();
}
[System.Serializable]
public class AllMapData
{
    public List<SaveRoomData> allMap = new();
}

