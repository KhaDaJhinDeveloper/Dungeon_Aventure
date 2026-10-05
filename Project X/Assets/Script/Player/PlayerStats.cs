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
    public override void Healing(float amount)
    {
        base.Healing(amount);
        UpdateUI();
    }
    public override void ArmorRecovery(float amount)
    {
        base.ArmorRecovery(amount);
        UpdateUI();
    }
    public override void AntiMagicRecovery(float amount)
    {
        base.AntiMagicRecovery(amount);
        UpdateUI();
    }
    public override void ManaRecovery(int amount)
    {
        base.ManaRecovery(amount); UpdateUI();
    }
    public override void ManaReduce(int amount)
    {
        base.ManaReduce(amount); UpdateUI();
    }
    public override void UpgradeMaxHealt(float amount)
    {
        base.UpgradeMaxHealt(amount); UpdateUI();
    }
    public override void UpgradeMaxArmor(float amount)
    {
        base.UpgradeMaxArmor(amount); UpdateUI();
    }
    public override void UpgradeMaxAntiMagic(float amount)
    {
        base.UpgradeMaxAntiMagic(amount); UpdateUI();
    }
    public override void UpgradeMaxMana(float amount)
    {
        base.UpgradeMaxMana(amount); UpdateUI();
    }
    protected override void Die()
    {
        base.Die();
        this.ani.AnimationDeath();
    }
    public override void UpdateUI()
    {
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadHPBar);
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadHPText);
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_loadArmorBar);
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadAntiMagicBar);
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadManaBar);
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadManaText);
    }
    protected override void KnockBack(Transform pos, float knockbackforce)
    {
        Vector2 direction = (this.transform.position - pos.transform.position).normalized;
        this.rb.velocity = direction * knockbackforce;
    }
}
