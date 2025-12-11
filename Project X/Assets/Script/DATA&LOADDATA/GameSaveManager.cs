using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : Singleton<GameSaveManager>
{
    private const string FILE_NAME = "PlayerDataPosition.json";
    private bool pendingLoad;
    public static GameSaveManager S_GameSaveManager { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void SaveAllDataLocal()
    {
        PlayerStatsDataManager.S_playerStatsDataManager.SaveData();
        PlayerPositionDataManager.S_playerPositionDataManager.SaveData();
        WeaponsDataManager.S_WeaponsDataManager.SaveData();
        GameStateCoinManager.S_GameStateCoinManager.SaveData();
        GameStateTimerManager.S_GameStateTimerManager.SaveData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventorySaveData);
        

        DebugLogger.Log("All game data Saved");
    }
    public void LoadAllDataLocal()
    {
        PlayerStatsDataManager.S_playerStatsDataManager.LoadData();
        PlayerPositionDataManager.S_playerPositionDataManager.LoadData();
        WeaponsDataManager.S_WeaponsDataManager.LoadData();
        GameStateCoinManager.S_GameStateCoinManager.LoadData();
        GameStateTimerManager.S_GameStateTimerManager.LoadData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventoryLoadData);


        DebugLogger.Log("All game data Loaded");
    }
    public void SaveDataWhenPlay()
    {
        PlayerStatsDataManager.S_playerStatsDataManager.SaveData();
        WeaponsDataManager.S_WeaponsDataManager.SaveData();
        GameStateCoinManager.S_GameStateCoinManager.SaveData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventorySaveData);

        DebugLogger.Log("All game data Loaded When Play");
    }
    public void LoadDataWhenPlay()
    {
        PlayerStatsDataManager.S_playerStatsDataManager.LoadData();
        WeaponsDataManager.S_WeaponsDataManager.LoadData();
        GameStateCoinManager.S_GameStateCoinManager.LoadData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventoryLoadData);

        DebugLogger.Log("All game data Loaded When Play");
    }
    public void DeleteAllDataLocal()
    {
        PlayerStatsDataManager.S_playerStatsDataManager.DeleteData();
        WeaponsDataManager.S_WeaponsDataManager.DeleteData();
        PlayerPositionDataManager.S_playerPositionDataManager.DeleteData();
        GameStateCoinManager.S_GameStateCoinManager.DeleteData();
        GameStateTimerManager.S_GameStateTimerManager.DeleteData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventoryDeleteData);
        DialogStateManager.dialogState_Instance.ResetAllStateDialog();
        DebugLogger.Log("All save data Deleted");
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_NAME);
    }
    private IEnumerator LoadAllDataWhenReady()
    {
        this.pendingLoad = false;
        yield return null;
        GameObject player = null;
        while (player == null)
        {
            player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
            if (player == null) yield return null;
        }
        yield return new WaitForFixedUpdate();
        LoadAllDataLocal();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            this.pendingLoad = false;
            return;
        }
        if (pendingLoad) StartCoroutine(LoadAllDataWhenReady());
    }
    public void RequestLoadOnNextScene() => this.pendingLoad = true;
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnApplicationQuit()
    {
        SaveAllDataLocal();
    }
}
