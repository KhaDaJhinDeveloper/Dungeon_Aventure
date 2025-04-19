using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStats : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    [SerializeField] protected int armor;
    [SerializeField] protected int antiMagic;
    [SerializeField] protected int speed;
    public int MaxHealth{ get => this.maxHealth; set => this.maxHealth = value; }
    public int CurentHealth{ get => this.currentHealth; set => this.currentHealth = Mathf.Clamp(value, 0 , maxHealth); }
    public int Speed{ get => this.speed; set => this.speed = value;}
    public int Armor{ get => this.armor; set => this.armor = Mathf.Max(value, 0); }
    public int AntiMagic { get => this.antiMagic; set => this.antiMagic = Mathf.Max(value, 0); }
    protected virtual void Start()
    {
        this.currentHealth = this.maxHealth;
        Debug.Log(currentHealth);
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
    protected virtual void Healing(int amount)
    {
        this.currentHealth += amount;       
    }
    protected virtual void ArmorRecovery(int amount)
    {
        this.armor += amount;
    }
    protected virtual void Die()
    {
        Debug.Log("Die");
    }

    //---------------------------------------------
    public class PhysicalDamage : IDamageType
    {
        public void ApplyDamage(BaseStats target, int amount)
        {
            target.ApplyPhysicalDamage(amount);
        }
    }
    public class MagicalDamage : IDamageType
    {
        public void ApplyDamage(BaseStats target, int amount)
        {
            target.ApplyMagicalDamage(amount);
        }
    }
    public class MixedDamage : IDamageType
    {
        private float physicalRate;
        public MixedDamage(float physicalRate)
        {
            this.physicalRate = Mathf.Clamp01(physicalRate);
        }
        public void ApplyDamage(BaseStats target, int amount)
        {
            this.physicalRate = Mathf.Clamp01(physicalRate);
            int physicalAmount = Mathf.RoundToInt(amount * physicalRate);
            int MagicalAmount = amount - physicalAmount;
            target.ApplyPhysicalDamage(physicalAmount);
            target.ApplyMagicalDamage(MagicalAmount);
        }
    }
    public class TrueDamage : IDamageType
    {
        public void ApplyDamage(BaseStats target, int amount)
        {
            target.ApplyTrueDamage(amount);
        }
    }
}
