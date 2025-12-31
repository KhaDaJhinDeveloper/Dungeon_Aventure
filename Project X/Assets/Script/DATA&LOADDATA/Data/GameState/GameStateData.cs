[System.Serializable]
public class GameStateSceneName
{
    public string sceneName;
    public GameStateSceneName() { }
    public GameStateSceneName(string sceneName) 
    { 
        this.sceneName = sceneName; 
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
