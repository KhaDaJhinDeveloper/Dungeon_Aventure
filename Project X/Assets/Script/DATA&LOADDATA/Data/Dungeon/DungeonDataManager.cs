using System.Collections.Generic;
using UnityEngine;

public class DungeonDataManager : Singleton<DungeonDataManager>, IDataManager
{
    #region FILE_NAME_DATA
    private const string FILE_DUNGEON_LAYOUT = "DungeonLayout.json";
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    public void SaveData()
    {
        MapGeneration[] maps = FindObjectsByType<MapGeneration>(FindObjectsSortMode.None);
        if (maps.Length == 0) return;
        AllMapData allMapsData = new AllMapData();
        foreach(var m in maps)
        {
            SaveRoomData saveDungeonData = new SaveRoomData();
            saveDungeonData.mapID = m.MapID;
            foreach (var roomData in m.gridMap)
            {
                saveDungeonData.listRoonNode.Add
                (
                     new RoomDataNode(roomData.Value.prefab.name, roomData.Key.x, roomData.Key.y)
                );
            }
            allMapsData.allMap.Add(saveDungeonData);
        }
        JsonFileUtility.SaveToJson<AllMapData>(allMapsData, FILE_DUNGEON_LAYOUT);
    }
    public void LoadData()
    {
        SaveRoomData saveDungeonData = JsonFileUtility.LoadFromJson<SaveRoomData>(FILE_DUNGEON_LAYOUT);
        if (saveDungeonData == null) return;
        MapGeneration[] maps = FindObjectsByType<MapGeneration>(FindObjectsSortMode.None);
        if(maps.Length == 0) return;


        DeleteData();
    }
    public void LoadData(MapGeneration map)
    {
        AllMapData allData = JsonFileUtility.LoadFromJson<AllMapData>(FILE_DUNGEON_LAYOUT);
        if (allData == null) return;
        foreach (var mapdata in allData.allMap)
        {
            if (mapdata.mapID == map.MapID)
            {
                foreach (var saveNode in mapdata.listRoonNode)
                {
                    Vector2Int pos = new Vector2Int(saveNode.x, saveNode.y);
                    if (map.backUp.TryGetValue(saveNode.roomPrefabName, out RoomData prefab))
                    {
                        map.gridMap.Add(pos, new PlacedRoomNode(prefab, pos));
                    }
                }
                map.RenderRoom();
            }
        }
    }    
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_DUNGEON_LAYOUT);
    }
    public bool HasMapData(string mapID)
    {
        if (!HasData()) return false;
        AllMapData data = GetMapData();
        return data.allMap.Exists(map => map.mapID == mapID);
    }
    private AllMapData GetMapData()
    {
        if (!HasData())
            return new AllMapData { allMap = new List<SaveRoomData>() };

        AllMapData data = JsonFileUtility.LoadFromJson<AllMapData>(FILE_DUNGEON_LAYOUT);

        if (data.allMap == null)
            data.allMap = new List<SaveRoomData>();

        return data;
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_DUNGEON_LAYOUT);
    }
}

