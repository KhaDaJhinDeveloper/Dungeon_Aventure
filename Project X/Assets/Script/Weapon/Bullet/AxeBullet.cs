using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

public class AxeBullet : BulletBase
{
    [SerializeField] private LayerMask nameLayerTarget;
    private IDamageType damageType;
    private void Start()
    {
        this.damageType = new MixedDamage(damagePhysicalRate);
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
        }
        ObjectPooling.ObjectPooling_Instance.ReturnToPool("AxeBullet", this.gameObject);
    }
    protected override void DamageAttack(BaseStats target)
    {
        target.TakeDamage(damage, damageType, this.transform);
    }
}
