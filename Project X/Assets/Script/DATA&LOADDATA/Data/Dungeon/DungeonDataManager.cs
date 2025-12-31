using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDataManager : Singleton<DungeonDataManager>, IDataManager
{
    #region FILE_NAME_DATA
    private const string FILE_DUNGEON_LAYOUT = "DungeonLayout.json";
    #endregion
    private Dictionary<string, DungeonLayoutData> allLayouts = new Dictionary<string, DungeonLayoutData>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void SaveData()
    {
        AllDungeonLayoutsDataSerializable serializable = new AllDungeonLayoutsDataSerializable();
        foreach (var kvp in allLayouts)
        {
            serializable.layouts.Add(new LayoutEntry { managerID = kvp.Key, layout = kvp.Value });
        }
        JsonFileUtility.SaveToJson(serializable, FILE_DUNGEON_LAYOUT);
    }

    public void LoadData()
    {
        AllDungeonLayoutsDataSerializable data = JsonFileUtility.LoadFromJson<AllDungeonLayoutsDataSerializable>(FILE_DUNGEON_LAYOUT);
        if (data == null)
        {
            DebugLogger.LogWarning("Dungeon layout data is null - creating empty layout");
            allLayouts = new Dictionary<string, DungeonLayoutData>();
            return;
        }
        
        // Load all data
        allLayouts = new Dictionary<string, DungeonLayoutData>();
        foreach (var entry in data.layouts)
        {
            if (!string.IsNullOrEmpty(entry.managerID))
            {
                allLayouts[entry.managerID] = entry.layout;
            }
        }
    }

    // Load Data current scene
    public void LoadData(string currentSceneName)
    {
        AllDungeonLayoutsDataSerializable data = JsonFileUtility.LoadFromJson<AllDungeonLayoutsDataSerializable>(FILE_DUNGEON_LAYOUT);
        if (data == null)
        {
            DebugLogger.LogWarning("Dungeon layout data is null - creating empty layout");
            allLayouts = new Dictionary<string, DungeonLayoutData>();
            return;
        }
        
        // Chỉ giữ lại data của scene đó, xóa data của scene khác
        allLayouts = new Dictionary<string, DungeonLayoutData>();
        string scenePrefix = currentSceneName + "_";
        foreach (var entry in data.layouts)
        {
            if (!string.IsNullOrEmpty(entry.managerID))
            {
                // Key format: "{sceneName}_{managerID}"
                // Chỉ giữ lại entry có key bắt đầu bằng "{currentSceneName}_"
                if (entry.managerID.StartsWith(scenePrefix))
                {
                    allLayouts[entry.managerID] = entry.layout;
                }
            }
        }
    }

    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_DUNGEON_LAYOUT);
        allLayouts = new Dictionary<string, DungeonLayoutData>();
    }

    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_DUNGEON_LAYOUT);
    }

    //Methods để lấy/lưu layout của một RoomManager cụ thể
    private string ComposeKey(string sceneName, string managerID)
    {
        return $"{sceneName}_{managerID}";
    }

    public DungeonLayoutData GetLayout(string sceneName, string managerID)
    {
        string key = ComposeKey(sceneName, managerID);
        if (allLayouts.ContainsKey(key))
        {
            return allLayouts[key];
        }
        return null;
    }

    public void SetLayout(string sceneName, string managerID, DungeonLayoutData layout)
    {
        string key = ComposeKey(sceneName, managerID);
        allLayouts[key] = layout;
    }
}

