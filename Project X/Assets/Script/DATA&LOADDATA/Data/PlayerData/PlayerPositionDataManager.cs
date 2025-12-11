using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionDataManager : Singleton<PlayerPositionDataManager>, IDataManager
{
    public static PlayerPositionDataManager S_playerPositionDataManager { get; private set; }
    #region FILE_NAME
    private const string FILE_PLAYER_DATA_POSITION = "PlayerDataPosition.json";
    private PlayerPositionData positionData = new PlayerPositionData();
    #endregion
    protected override void Awake()
    {
        base.Awake();
        S_playerPositionDataManager = this;
    }
    public void SaveData()
    {
        GameObject player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
        if (player == null) return;
        this.positionData = new PlayerPositionData(player.transform.position);
        JsonFileUtility.SaveToJson(this.positionData, FILE_PLAYER_DATA_POSITION);
    }
    public void LoadData()
    {
        PlayerPositionData positionData = JsonFileUtility.LoadFromJson<PlayerPositionData>(FILE_PLAYER_DATA_POSITION);
        PlayerStats playerStats = GameObject.FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            DebugLogger.LogWarning("PlayerStats not found - cannot load data");
            return;
        }
        playerStats.transform.position = positionData.position;
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_PLAYER_DATA_POSITION);
        this.positionData = new PlayerPositionData();
    }

}
