using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneStateManager : Singleton<GameSceneStateManager>,IDataManager
{
    private const string FILE_GAMESATE_SCENENAME = "SceneNameData.json";
    private GameStateSceneName gameSceneName = new GameStateSceneName();
    public static GameSceneStateManager S_GameSceneStateManager { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        S_GameSceneStateManager = this;
    }
    public void SaveData()
    {
        string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        this.gameSceneName = new GameStateSceneName(name);
        JsonFileUtility.SaveToJson(this.gameSceneName, FILE_GAMESATE_SCENENAME);
        DebugLogger.Log($"Scene name saved {name}");
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
    public bool HasData()
    {
        return false;
    }
}
