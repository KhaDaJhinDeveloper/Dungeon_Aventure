using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private enum Status
    {
        Normal, Hungry, Thirsty
    }
    private Status player_status;
    [SerializeField] private float maxStartingTime;
    private float currentTime = 0;
    private PlayerStatusManager statusManager;
    public float MaxStartingTime { get => maxStartingTime; set => maxStartingTime = value; }
    public float CurrentTime { get => currentTime; set => currentTime = value; }

    void Start()
    {
        this.currentTime = this.maxStartingTime;
        this.statusManager = GetComponent<PlayerStatusManager>();
    }
    void Update()
    {
        RunTime();
        EventManager.OP_EventManager.TriggerEvent("LoadTimeBar");
    }
    void RunTime()
    {
        this.currentTime -= Time.deltaTime;
        if (this.currentTime > this.maxStartingTime * 0.5f) player_status = Status.Normal;
        if (this.currentTime <= this.maxStartingTime * 0.5f) player_status = Status.Hungry;
        if (this.currentTime <= 0)
        {
            player_status = Status.Thirsty;
            this.currentTime = 0;
        }
        switch(this.player_status)
        {
            case Status.Normal:
                this.statusManager.StatusNormal(); 
                break;
            case Status.Hungry:
                this.statusManager.StatusHungry();
                break;
            case Status.Thirsty:
                this.statusManager.StatusThirsty();
                break;
        }
    }
    public void IncreaseTime(float amount)
    {
        this.currentTime += this.maxStartingTime * amount;
        if (this.currentTime > this.maxStartingTime) this.currentTime = this.maxStartingTime;
    }    
}
