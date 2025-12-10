using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateTimerManager : Singleton<GameStateTimerManager>, IDataManager
{
    public static GameStateTimerManager S_GameStateTimerManager { get; private set; }
    #region FILE_NAME
    public const string FILE_GAMESTATE_TIMERDATA = "TimerData.json";
    #endregion
    private GameStateTimer gameStateTimer = new GameStateTimer();
    protected override void Awake()
    {
        base.Awake();
        S_GameStateTimerManager = this;
    }
    public void SaveData()
    {
        CountdownTimer countdownTimer = GameObject.FindFirstObjectByType<CountdownTimer>();
        if (countdownTimer != null)
        {
            this.gameStateTimer = new GameStateTimer(countdownTimer.MaxStartingTime, countdownTimer.CurrentTime);
            JsonFileUtility.SaveToJson(this.gameStateTimer, FILE_GAMESTATE_TIMERDATA);
        }
        else DebugLogger.Log("CountdownTimer not found");
    }
    public void LoadData()
    {
        GameStateTimer gameStateTimer = JsonFileUtility.LoadFromJson<GameStateTimer>(FILE_GAMESTATE_TIMERDATA);
        if (gameStateTimer == null)
        {
            DebugLogger.LogWarning("Failed to load gameStateTimer data - data is null");
            return;
        }
        CountdownTimer countdownTimer = GameObject.FindFirstObjectByType<CountdownTimer>();
        if(countdownTimer == null)
        {
            DebugLogger.LogWarning("Failed to load CountdownTimer - CoinManager is null");
            return;
        }
        countdownTimer.MaxStartingTime = gameStateTimer.maxTime;
        countdownTimer.CurrentTime = gameStateTimer.currentTime;
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_GAMESTATE_TIMERDATA);
        this.gameStateTimer = new GameStateTimer();
    }
}
