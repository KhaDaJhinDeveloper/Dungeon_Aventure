using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private int speedOriginal;

    void Start()
    {
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
        IndexDefault();
    }
    public void StatusNormal()
    {
        this.playerStats.Speed = this.speedOriginal;
    }    
    public void StatusHungry()
    {
        this.playerStats.Speed = 4;
    }
    public void StatusThirsty()
    {
        //Debug.Log("Thirsty");
    }    
    void IndexDefault()
    {
        this.speedOriginal = this.playerStats.Speed;    
    }    
}
