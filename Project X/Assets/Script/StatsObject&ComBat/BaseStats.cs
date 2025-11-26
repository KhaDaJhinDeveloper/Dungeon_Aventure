using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseStats : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    protected bool isDie;
    protected int armor;
    protected int antiMagic;
    [SerializeField] protected int maxArmor;
    [SerializeField] protected int maxAntiMagic;
    [SerializeField] protected int speed;
    public int MaxHealth{ get => this.maxHealth; set => this.maxHealth = value; }
    public int CurentHealth{ get => this.currentHealth; set => this.currentHealth = Mathf.Clamp(value, 0 , maxHealth); }
    public int Speed{ get => this.speed; set => this.speed = value;}
    public int Armor{ get => this.armor; set => this.armor = Mathf.Max(value, 0); }
    public int AntiMagic { get => this.antiMagic; set => this.antiMagic = Mathf.Max(value, 0); }
    public bool IsDie { get => isDie; set => isDie = value; }
    public int MaxArmor { get => maxArmor; set => this.maxArmor = Mathf.Max(value, 0); }
    public int MaxAntiMagic { get => maxAntiMagic; set => this.maxAntiMagic = Mathf.Max(value, 0); }

    protected virtual void Start()
    {
        this.currentHealth = this.maxHealth;
        this.armor = this.maxArmor;
        this.antiMagic = this.maxAntiMagic;
    }
    public virtual void TakeDamage(int amount, IDamageType damageType, Transform pos)
    {
        damageType.ApplyDamage(this, amount);
    }
    public virtual void ApplyPhysicalDamage(int amount)
    {
        if (this.isDie) return;
        if (this.armor > 0)
        {
            int armorDamage = Mathf.Min(this.armor, amount);
            this.armor -= armorDamage;
            amount -= armorDamage;
        }
        if(amount > 0)
        {
            this.currentHealth -= amount;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDie = true;
                Die();
            }
        }
    }
    public virtual void ApplyMagicalDamage(int amount)
    {
        if (this.isDie) return;
        if (this.antiMagic > 0)
        {
            int magicDamage = Mathf.Min(this.antiMagic, amount);
            this.antiMagic -= magicDamage;
            amount -= magicDamage;
        }
        if (amount > 0)
        {
            this.currentHealth -= amount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDie = true;
                Die();
            }
        }
    }
    public virtual void ApplyTrueDamage(int amount)
    {
        if (this.isDie) return;
        this.currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDie = true;
            Die();
        }
    }    
    public virtual void Healing(int amount)
    {
        this.currentHealth += amount;   
        if(this.currentHealth >= this.maxHealth)
            this.currentHealth = this.maxHealth;
    }
    public virtual void ArmorRecovery(int amount)
    {
        this.armor += amount;
        if (this.armor >= this.maxArmor)
            this.armor = this.maxArmor;
    }
    public virtual void AntiMagicRecovery(int amount)
    {
        this.antiMagic += amount;
        if (this.antiMagic >= this.maxAntiMagic)
            this.antiMagic = this.maxAntiMagic;
    }
    protected virtual void Die()
    {
        
    }  
    protected virtual void ResetStats()
    {
        this.currentHealth = this.maxHealth;
        this.armor = this.maxArmor;
        this.antiMagic = this.maxAntiMagic;
    }
    protected virtual void OnEnable()
    {
        this.currentHealth = this.maxHealth;
        this.armor = this.maxArmor;
        this.antiMagic = this.maxAntiMagic;
        this.isDie = false;
    }
    protected virtual void KnockBack(Transform pos, float knockbackforce)
    {

    }
}
