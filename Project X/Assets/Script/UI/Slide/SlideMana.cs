using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMana : BaseSlider
{
    [SerializeField] private BaseStats stats;
    protected override void Start()
    {
        base.Start();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_LoadManaBar, Load);
    }
    public override void Load()
    {
        if (this.stats.MaxMana <= 0)
        {
            sliderBar.fillAmount = 0f;
        }
        else
            sliderBar.fillAmount = (float)this.stats.Mana / this.stats.MaxMana;
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_LoadManaBar, Load);
    }
}
