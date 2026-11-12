using UnityEngine;

public class SlideScaleTime : BaseSlider
{
    private CountdownTimer countdownTimer;
    private float currentTime;
    private float maxStartingTime;
    protected override void Start()
    {
        base.Start();
        this.countdownTimer = GameObject.FindFirstObjectByType<CountdownTimer>();
        EventManager.Instance.Subscribe("LoadTimeBar", Load);
    }
    public override void Load()
    {
        this.currentTime = this.countdownTimer.CurrentTime;
        this.maxStartingTime = this.countdownTimer.MaxStartingTime;
        sliderBar.fillAmount = (float)this.currentTime / this.maxStartingTime;
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe("LoadTimeBar", Load);
    }
}
