using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAntiMagic : BaseSlider
{
    [SerializeField] private BaseStats stats;
    protected override void Start()
    {
        base.Start();
        EventManager.Instance.Subscribe(NameEvent.Event_LoadAntiMagicBar, Load);
    }
    public override void Load()
    {
        if (this.stats.MaxAntiMagic <= 0)
        {
            sliderBar.fillAmount = 0f;
        }
        else
            sliderBar.fillAmount = (float)this.stats.AntiMagic / this.stats.MaxAntiMagic;
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe(NameEvent.Event_LoadAntiMagicBar, Load);
    }
}
