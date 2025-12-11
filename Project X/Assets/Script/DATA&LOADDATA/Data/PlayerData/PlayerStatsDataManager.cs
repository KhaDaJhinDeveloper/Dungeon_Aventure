
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsDataManager : Singleton<PlayerStatsDataManager>,IDataManager
{
    public static PlayerStatsDataManager S_playerStatsDataManager {  get; private set; }
    #region FILE_NAME
    private const string FILE_PLAYER_DATA_STATS = "PlayerDataStats.json";
    #endregion
    private PlayerStatsData statsData = new PlayerStatsData();


    protected override void Awake()
    {
        base.Awake();
        S_playerStatsDataManager = this;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void SaveData()
    {
        GameObject player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
        if (player == null) return;
        PlayerStats stats = player.GetComponent<PlayerStats>();

        this.statsData = new PlayerStatsData(stats.MaxHealth,
                                             stats.CurentHealth,
                                             stats.Speed, stats.Armor,
                                             stats.AntiMagic);
        JsonFileUtility.SaveToJson(this.statsData, FILE_PLAYER_DATA_STATS);
    }

    public void LoadData()
    {
        PlayerStatsData stats = JsonFileUtility.LoadFromJson<PlayerStatsData>(FILE_PLAYER_DATA_STATS);
        if (stats == null )
        {
            DebugLogger.LogWarning("Failed to load player data - data is null");
            return;
        }

        PlayerStats playerStats = GameObject.FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            DebugLogger.LogWarning("PlayerStats not found - cannot load data");
            return;
        }
        playerStats.MaxHealth = stats.maxHeal;
        playerStats.CurentHealth = stats.currentHeal;
        playerStats.Speed = stats.speed;
        playerStats.Armor = stats.armor;
        playerStats.AntiMagic = stats.antiMagic;
        playerStats.UpdateUI();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            LoadData();
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_PLAYER_DATA_STATS);
        this.statsData = new PlayerStatsData();
    }
}
