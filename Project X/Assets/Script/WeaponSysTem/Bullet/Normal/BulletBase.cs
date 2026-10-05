using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class 
    BulletBase : MonoBehaviour
{
    [SerializeField] protected KeyPool keyPool;
    protected int damageMain;
    protected float rateMixDamage;
    protected IDamageType damageType;
    protected Rigidbody2D rb;
    public virtual void InitializeFromWeapon(WeaponInstance instance)
    {
        this.damageType = instance.damageType;
        this.damageMain = instance.mainDamage;
        this.rateMixDamage = instance.rateMixDamage;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void DamageAttack(BaseStats target)
    {

    }
    public void BulletLauch(WeaponInstance weaponInstance,float speed)
    {
        InitializeFromWeapon(weaponInstance);
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }    
}
