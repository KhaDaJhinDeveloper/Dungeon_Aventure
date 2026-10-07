using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : Singleton<GameSaveManager>
{
    private const string FILE_NAME = "PlayerDataPosition.json";
    private bool pendingLoad;
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void SaveAllDataLocal()
    {
        PlayerStatsDataManager.Instance.SaveData();
        PlayerPositionDataManager.Instance.SaveData();
        WeaponsSaveLoad.Instance.SaveData();
        GameStateCoinManager.Instance.SaveData();
        GameStateTimerManager.Instance.SaveData();
        GameSceneStateManager.Instance.SaveData();
        DungeonDataManager.Instance.SaveData();
        //EnemiesDataManager.Instance.SaveData();
        ItemDataManager.Instance.SaveData();
        EventManager.Instance.TriggerEvent(NameEvent.Event_InventorySaveData);
        DebugLogger.Log("All game data Saved");
    }
    public void LoadAllDataLocal()
    {
        PlayerStatsDataManager.Instance.LoadData();
        PlayerPositionDataManager.Instance.LoadData();
        WeaponsSaveLoad.Instance.LoadData();
        GameStateCoinManager.Instance.LoadData();
        GameStateTimerManager.Instance.LoadData();

        string currentSceneName = SceneManager.GetActiveScene().name;
        //EnemiesDataManager.Instance.LoadData(currentSceneName);
        ItemDataManager.Instance.LoadData();
        EventManager.Instance.TriggerEvent(NameEvent.Event_InventoryLoadData);


        DebugLogger.Log("All game data Loaded");
    }
    public void SaveDataWhenPlay()
    {
        PlayerStatsDataManager.Instance.SaveData();
        WeaponsSaveLoad.Instance.SaveData();
        GameStateCoinManager.Instance.SaveData();
        EnemiesDataManager.Instance.DeleteData();
        ItemDataManager.Instance.DeleteData();
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_InventorySaveData);
        DebugLogger.Log("All game data Saved When Play");
    }
    public void LoadDataWhenPlay()
    {
        StartCoroutine(LoadDataWhenPlayWhenReady());
    }
    public void DeleteAllDataLocal()
    {
        PlayerStatsDataManager.Instance.DeleteData();
        WeaponsSaveLoad.Instance.DeleteData();
        PlayerPositionDataManager.Instance.DeleteData();
        GameStateCoinManager.Instance.DeleteData();
        GameStateTimerManager.Instance.DeleteData();
        GameSceneStateManager.Instance.DeleteData();
        DungeonDataManager.Instance.DeleteData();
        EnemiesDataManager.Instance.DeleteData();
        EventManager.Instance.TriggerEvent(NameEvent.Event_InventoryDeleteData);
        DialogStateManager.dialogState_Instance.ResetAllStateDialog();
        DebugLogger.Log("All save data Deleted");
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
    private IEnumerator LoadDataWhenPlayWhenReady()
    {
        GameObject player = null;
        while (player == null)
        {
            player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
            if (player == null) yield return null;
        }
        yield return new WaitForFixedUpdate();
        PlayerStatsDataManager.Instance.LoadData();
        WeaponsSaveLoad.Instance.LoadData();
        GameStateCoinManager.Instance.LoadData();
        EventManager.Instance.TriggerEvent(NameEvent.Event_InventoryLoadData);
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" )
        {
            this.pendingLoad = false;
            return;
        }
        if (pendingLoad) StartCoroutine(LoadAllDataWhenReady());
    }
    public void RequestLoadOnNextScene() => this.pendingLoad = true;
    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_NAME);
    }
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
