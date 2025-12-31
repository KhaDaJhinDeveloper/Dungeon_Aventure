using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInstanceData
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
    public bool[] exitsUsed;
}

[System.Serializable]
public class DungeonLayoutData
{
    public List<RoomInstanceData> rooms = new List<RoomInstanceData>();
}

[System.Serializable]
public class LayoutEntry
{
    public string managerID;
    public DungeonLayoutData layout;
}

[System.Serializable]
public class AllDungeonLayoutsDataSerializable
{
    public List<LayoutEntry> layouts = new List<LayoutEntry>();
}


