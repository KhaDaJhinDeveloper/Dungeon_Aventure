using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCoinManager : Singleton<GameStateCoinManager>, IDataManager
{
    public static GameStateCoinManager S_GameStateCoinManager { get; private set; }
    #region FILE_NAME
    public const string FILE_GAMESTATE_COINDATA = "CoinData.json";
    #endregion
    private GameStateCoin gameStateCoin = new GameStateCoin();
    protected override void Awake()
    {
        base.Awake();
        S_GameStateCoinManager = this;
    }
    public void SaveData()
    {
        CoinManager coinmanager = GameObject.FindFirstObjectByType<CoinManager>();
        if (coinmanager != null)
        {
            gameStateCoin = new GameStateCoin(coinmanager.CoinAmount);
            JsonFileUtility.SaveToJson(this.gameStateCoin, FILE_GAMESTATE_COINDATA);
        }
        else return;
    }
    public void LoadData()
    {
        GameStateCoin gameStateCoin = JsonFileUtility.LoadFromJson<GameStateCoin>(FILE_GAMESTATE_COINDATA);
        if (gameStateCoin == null)
        {
            DebugLogger.LogWarning("Failed to load gameStateCoin data - data is null");
            return;
        }
        CoinManager coinmanager = GameObject.FindFirstObjectByType<CoinManager>();
        if (coinmanager == null)
        {
            DebugLogger.LogWarning("Failed to load CoinManager - CoinManager is null");
            return;
        }
        coinmanager.CoinAmount = gameStateCoin.coinAmount;
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_GAMESTATE_COINDATA);
        gameStateCoin = new GameStateCoin();
    }
    public bool HasData()
    {
        return false;
    }
}
