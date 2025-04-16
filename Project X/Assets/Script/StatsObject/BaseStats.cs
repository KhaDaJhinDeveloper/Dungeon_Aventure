using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStats : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
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
    }
    protected virtual void TakeDamage(int amount)
    {
        if(this.armor > 0) this.armor -= amount;
        else
            this.currentHealth -= amount;
        if (this.currentHealth <= 0) Die();
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

    }
}
