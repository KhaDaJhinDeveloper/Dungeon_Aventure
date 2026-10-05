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
    protected int mana;
    [SerializeField] protected int maxArmor;
    [SerializeField] protected int maxAntiMagic;
    [SerializeField] protected int maxMana;
    [SerializeField] protected int speed;
    [SerializeField] protected KeyPool key;
    public int MaxHealth{ get => this.maxHealth; set => this.maxHealth = value; }
    public int CurentHealth{ get => this.currentHealth; set => this.currentHealth = Mathf.Clamp(value, 0 , maxHealth); }
    public int Speed{ get => this.speed; set => this.speed = value;}
    public int Armor{ get => this.armor; set => this.armor = Mathf.Max(value, 0); }
    public int AntiMagic { get => this.antiMagic; set => this.antiMagic = Mathf.Max(value, 0); }
    public int Mana { get => this.mana; set => this.mana = Mathf.Max(value, 0); }
    public bool IsDie { get => isDie; set => isDie = value; }
    public int MaxArmor { get => maxArmor; set => this.maxArmor = Mathf.Max(value, 0); }
    public int MaxAntiMagic { get => maxAntiMagic; set => this.maxAntiMagic = Mathf.Max(value, 0); }
    public int MaxMana { get => maxMana; set => this.maxMana = Mathf.Max(value, 0); }
    public KeyPool Key { get => this.key; }

    protected virtual void Start()
    {
        this.currentHealth = this.maxHealth;
        this.armor = this.maxArmor;
        this.antiMagic = this.maxAntiMagic;
        this.mana = this.maxMana;
    }
    #region TakeDamgeMethod
    public virtual void TakeDamage(int amount, IDamageType damageType, Transform pos)
    {
        damageType.ApplyDamage(this, amount);
    }
    public virtual void ApplyPhysicalDamage(int amount)
    {
        if (ThisIsDie()) return;
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
        if (ThisIsDie()) return;
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
        if (ThisIsDie()) return;
        this.currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDie = true;
            Die();
        }
    }
    #endregion
    #region StatsInteract
    public virtual void Healing(float amount)
    {
        this.currentHealth += Mathf.RoundToInt( this.maxHealth*amount );   
        if(this.currentHealth >= this.maxHealth)
            this.currentHealth = this.maxHealth;
    }
    public virtual void ArmorRecovery(float amount)
    {
        this.armor += Mathf.RoundToInt( this.maxArmor*amount );
        if (this.armor >= this.maxArmor)
            this.armor = this.maxArmor;
    }
    public virtual void AntiMagicRecovery(float amount)
    {
        this.antiMagic += Mathf.RoundToInt( this.MaxAntiMagic * amount);
        if (this.antiMagic >= this.maxAntiMagic)
            this.antiMagic = this.maxAntiMagic;
    }
    public virtual void ManaRecovery(int amount)
    {
        this.mana +=  amount;
        if (this.mana >= this.maxMana)
            this.mana = this.maxMana;
    }
    public virtual void ManaReduce(int amount)
    {
        this.mana -=  amount;
        if (this.mana <= 0)
            this.mana = 0;
    }
    #region UpgradeStats
    public virtual void UpgradeMaxHealt(float amount)
    {
        amount = Mathf.Clamp01(amount);
        this.maxHealth += Mathf.RoundToInt( this.maxHealth * amount);
    }
    public virtual void UpgradeMaxArmor(float amount)
    {
        amount = Mathf.Clamp01(amount);
        this.maxArmor += Mathf.RoundToInt(this.maxArmor * amount);
    }
    public virtual void UpgradeMaxAntiMagic(float amount)
    {
        amount = Mathf.Clamp01(amount);
        this.maxAntiMagic += Mathf.RoundToInt(this.maxAntiMagic * amount);
    }
    public virtual void UpgradeMaxMana(float amount)
    {
        amount = Mathf.Clamp01(amount);
        this.maxMana += Mathf.RoundToInt(this.maxMana * amount);
    }
    #endregion
    #endregion
    public virtual bool ThisIsDie() => this.currentHealth <= 0;
    protected virtual void Die()
    {
        
    }  
    //protected virtual void ResetStats()
    //{
    //    this.currentHealth = this.maxHealth;
    //    this.armor = this.maxArmor;
    //    this.antiMagic = this.maxAntiMagic;
    //}
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
    public virtual void UpdateUI()
    {

    }    
}
