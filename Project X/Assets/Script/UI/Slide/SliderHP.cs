using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHP : BaseSlider
{
    [SerializeField] private BaseStats BaseStats;
    private int currentHealth;
    private int maxHealth;
    protected override void Start()
    {
        base.Start();
        EventManager.OP_EventManager.Subscribe("LoadHp",Load);
    }
    public override void Load()
    {
        this.currentHealth = BaseStats.CurentHealth;
        this.maxHealth = BaseStats.MaxHealth;
        sliderBar.fillAmount = (float)this.currentHealth/this.maxHealth;
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("LoadHp", Load);
    }
}
