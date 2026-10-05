using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected string nameWeapon;
    protected IDamageType damageType;
    protected Animator ani;
    protected SpriteRenderer srWeapon;
    protected Sprite srOriginal;
    [SerializeField] protected KeyPool key;
    protected virtual void Start()
    {
        this.ani = GetComponent<Animator>();
        this.srWeapon = GetComponent<SpriteRenderer>();
        this.srOriginal = this.srWeapon.sprite;
        this.nameWeapon = gameObject.name;
    }
    [SerializeField] protected int magical;
    [SerializeField] protected int strength;
    public int Magical { get => this.magical; set => this.magical = value; }
    public int Strength { get => this.strength; set => this.strength = value; }
    public Sprite SrOriginal { get => this.srOriginal; }
    public string NameWeapon { get => this.nameWeapon; }
    public KeyPool Key { get => this.key; }

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
