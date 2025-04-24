using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStats : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    protected bool isDie;
    [SerializeField] protected int armor;
    [SerializeField] protected int antiMagic;
    [SerializeField] protected int speed;
    public int MaxHealth{ get => this.maxHealth; set => this.maxHealth = value; }
    public int CurentHealth{ get => this.currentHealth; set => this.currentHealth = Mathf.Clamp(value, 0 , maxHealth); }
    public int Speed{ get => this.speed; set => this.speed = value;}
    public int Armor{ get => this.armor; set => this.armor = Mathf.Max(value, 0); }
    public int AntiMagic { get => this.antiMagic; set => this.antiMagic = Mathf.Max(value, 0); }
    public bool IsDie { get => isDie; set => isDie = value; }

    protected virtual void Start()
    {
        this.currentHealth = this.maxHealth;
    }
    public virtual void TakeDamage(int amount, IDamageType damageType)
    {
        damageType.ApplyDamage(this, amount);
    }
    public virtual void ApplyPhysicalDamage(int amount)
    {
        if(this.armor > 0)
        {
            int armorDamage = Mathf.Min(this.armor, amount);
            this.armor -= armorDamage;
            amount -= armorDamage;
        }
        if(amount > 0)
        {
            this.currentHealth -= amount;
            if(currentHealth <= 0) Die();
        }
    }
    public virtual void ApplyMagicalDamage(int amount)
    {
        if (this.antiMagic > 0)
        {
            int magicDamage = Mathf.Min(this.antiMagic, amount);
            this.antiMagic -= magicDamage;
            amount -= magicDamage;
        }
        if (amount > 0)
        {
            this.currentHealth -= amount;
            if (currentHealth <= 0) Die();
        }
    }
    public virtual void ApplyTrueDamage(int amount)
    {
        this.currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }    
    public virtual void Healing(int amount)
    {
        this.currentHealth += amount;       
    }
    public virtual void ArmorRecovery(int amount)
    {
        this.armor += amount;
    }
    protected virtual void Die()
    {
        isDie = true;
    }  
}
