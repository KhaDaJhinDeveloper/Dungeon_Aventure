using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneStateManager : Singleton<GameSceneStateManager>,IDataManager
{
    private const string FILE_GAMESATE_SCENENAME = "SceneNameData.json";
    private GameStateSceneName gameSceneName = new GameStateSceneName();
    public void SaveData()
    {
        CutSceneRunner cutScene = FindAnyObjectByType<CutSceneRunner>();
        SceneBossState bossScene = FindAnyObjectByType<SceneBossState>();
        if (cutScene != null && bossScene != null) 
            this.gameSceneName.bossState = new BossState(cutScene.CutSceneID, cutScene.canInteract, bossScene.isEnd);
        string name = SceneExtensions.GetCurrentSceneName();
        this.gameSceneName.sceneName = name;
        JsonFileUtility.SaveToJson(this.gameSceneName, FILE_GAMESATE_SCENENAME);
    }
    public void LoadData()
    {
        //Do not use
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_GAMESATE_SCENENAME);
        this.gameSceneName = new GameStateSceneName();
    }
    public string GetSceneNameData()
    {
        GameStateSceneName dataName = JsonFileUtility.LoadFromJson<GameStateSceneName>(FILE_GAMESATE_SCENENAME);
        if (dataName == null)
        {
            DebugLogger.LogWarning("Failed to load gameStateNameScene data - data is null");
            return "";
        }
        return dataName.sceneName;
    }
    public bool HasCutSceneID(string ID)
    {
        GameStateSceneName stateData = JsonFileUtility.LoadFromJson<GameStateSceneName>(FILE_GAMESATE_SCENENAME);
        if (stateData == null) return false;
        if (string.IsNullOrEmpty(stateData.bossState.cutSceneId)) return false;
        return ID == stateData.bossState.cutSceneId;
    }
    public GameStateSceneName GetData()
    {
        if (!HasData())
            return new GameStateSceneName ();
        GameStateSceneName data = JsonFileUtility.LoadFromJson<GameStateSceneName>(FILE_GAMESATE_SCENENAME);
        return data;
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_GAMESATE_SCENENAME);
    }
}
