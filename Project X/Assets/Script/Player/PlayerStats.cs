using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : BaseStats
{
    private PlayerAnimation ani;
    private Rigidbody2D rb;
    protected override void Start()
    {
        base.Start();
        this.ani = GetComponent<PlayerAnimation>();
        this.rb = GetComponent<Rigidbody2D>();
        UpdateUI();
    }
    public override void TakeDamage(int amount, IDamageType damageType, Transform pos)
    {       
        base.TakeDamage(amount, damageType, pos);
        UpdateUI();
        if (currentHealth > 0)
        {
            this.ani.AnimationTakeHit();
            KnockBack(this.transform, 50f);
        }
        else
            Die();
    }
    public override void Healing(int amount)
    {
        base.Healing(amount);
        UpdateUI();
    }
    public override void ArmorRecovery(int amount)
    {
        base.ArmorRecovery(amount);
        UpdateUI();
    }
    public override void AntiMagicRecovery(int amount)
    {
        base.AntiMagicRecovery(amount);
        UpdateUI();
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
    protected override void KnockBack(Transform pos, float knockbackforce)
    {
        Vector2 direction = (this.transform.position - pos.transform.position).normalized;
        this.rb.velocity = direction * knockbackforce;
    }
}
