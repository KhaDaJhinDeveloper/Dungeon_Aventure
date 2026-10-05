using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderArmor : BaseSlider
{
    [SerializeField] private BaseStats stats;
    protected override void Start()
    {
        base.Start();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_loadArmorBar, Load);
    }
    public override void Load()
    {
        if (this.stats.MaxArmor <= 0)
        {
            sliderBar.fillAmount = 0f;
        }
        else
            sliderBar.fillAmount = (float)this.stats.Armor/this.stats.MaxArmor;
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_loadArmorBar, Load);
    }
}
