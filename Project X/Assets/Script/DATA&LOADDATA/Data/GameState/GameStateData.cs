[System.Serializable]
public class GameStateSceneName
{
    public string sceneName;
    public BossState bossState;
    public GameStateSceneName() { }
    public GameStateSceneName(string sceneName) 
    { 
        this.sceneName = sceneName; 
    }
}

[System.Serializable]
public class BossState
{
    public string cutSceneId;
    public bool canInteract;
    public bool isEnd;
    public BossState() { }
    public BossState(string cutsceneID, bool canInteract, bool isEnd)
    {
        this.cutSceneId = cutsceneID;
        this.canInteract = canInteract;
        this.isEnd = isEnd;
    }
}

[System.Serializable]
public class GameStateTimer
{
    public float maxTime;
    public float currentTime;
    public GameStateTimer() { }
    public GameStateTimer(float maxtime, float currenttime) 
    { 
        this.maxTime = maxtime;
        this.currentTime = currenttime;
    }
}

[System.Serializable]
public class GameStateCoin
{
    public int coinAmount;
    public GameStateCoin() { }
    public GameStateCoin(int coinamount) 
    {
        this.coinAmount = coinamount;
    }
}
