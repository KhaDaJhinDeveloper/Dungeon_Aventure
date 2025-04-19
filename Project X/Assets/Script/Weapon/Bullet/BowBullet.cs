using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

public class BowBullet : BulletBase
{
    [SerializeField] private LayerMask nameLayerTarget;
    private IDamageType damageType;
    private void Start()
    {
        damageType = new PhysicalDamage();  
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & nameLayerTarget.value) > 0)
        {
            BaseStats baseStats = collision.gameObject.GetComponentInChildren<BaseStats>();
            if (baseStats != null)
            {
                DamageAttack(baseStats);
            }    
            ObjectPooling.ObjectPooling_Instance.ReturnToPool("BowBullet", this.gameObject);
        }          
    }
    protected override void DamageAttack(BaseStats taget)
    {
        taget.TakeDamage(damage, damageType);
    }
}
