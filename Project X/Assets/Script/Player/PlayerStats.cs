using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    private PlayerAnimation ani;
    protected override void Start()
    {     
        base.Start();
        this.ani = GetComponent<PlayerAnimation>();
        UpdateUI();
    }
    public override void TakeDamage(int amount, IDamageType damageType)
    {       
        base.TakeDamage(amount, damageType);
        UpdateUI();
        if (currentHealth > 0)
            this.ani.AnimationTakeHit();
        else
            Die();
    }
    protected override void Die()
    {
        base.Die();
        this.ani.AnimationDeath();
    }
    void UpdateUI()
    {
        EventManager.OP_EventManager.TriggerEvent("LoadHp");
        EventManager.OP_EventManager.TriggerEvent("LoadHPText");
        EventManager.OP_EventManager.TriggerEvent("LoadArmor");
        EventManager.OP_EventManager.TriggerEvent("LoadAntimagic");
    }
}
