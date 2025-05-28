using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
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
        if (this.currentTime > this.maxStartingTime * 0.5f) this.statusManager.StatusNormal();
        if (this.currentTime <= this.maxStartingTime * 0.5f) this.statusManager.StatusHungry();
        if (this.currentTime <= 0)
        {
            this.statusManager.StatusThirsty();
            this.currentTime = 0;
        }
    }
    public void IncreaseTime(float amount)
    {
        this.currentTime += this.maxStartingTime * amount;
        if (this.currentTime > this.maxStartingTime) this.currentTime = this.maxStartingTime;
    }    
}
