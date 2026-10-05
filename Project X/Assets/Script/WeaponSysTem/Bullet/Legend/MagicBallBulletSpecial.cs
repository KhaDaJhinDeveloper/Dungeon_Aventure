using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallBulletSpecial : BulletBase
{
    [SerializeField] private LayerMask nameLayerTarget;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & nameLayerTarget.value) > 0)
        {
            BaseStats baseStats = collision.gameObject.GetComponentInChildren<BaseStats>();
            if (baseStats != null)
            {
                DamageAttack(baseStats);
            }
            ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.keyPool, this.gameObject);
        }
    }
    protected override void DamageAttack(BaseStats target)
    {
        target.TakeDamage(this.damageMain, this.damageType, this.transform);
    }
    public override void InitializeFromWeapon(WeaponInstance instance)
    {
        this.damageType = instance.damageType;
        this.damageMain = instance.GetOppositeMainDamage();
        this.damageMain += Mathf.RoundToInt(this.damageMain * 0.5f);
        this.rateMixDamage = instance.rateMixDamage;
    }
}
