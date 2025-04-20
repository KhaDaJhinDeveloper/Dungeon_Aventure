using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected IDamageType damageType;
    protected WeaponManager weaponSlotAttack;
    protected Animator ani;
    protected virtual void Start()
    {
        this.ani = GetComponent<Animator>();
        this.weaponSlotAttack = GameObject.FindWithTag(TagManager.TAG_WEAPONSLOTS_ATTACK).GetComponent<WeaponManager>();
    }
    [SerializeField] protected int magical;
    [SerializeField] protected int strength;
    public int Magical { get => this.magical; set => this.magical = value; }
    public int Strength { get => this.strength; set => this.strength = value; }
    public void Initialize( int magical, int strength)
    {
        this.magical = magical;
        this.strength = strength;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    public virtual void DealDamage(BaseStats target)
    {

    }
}
