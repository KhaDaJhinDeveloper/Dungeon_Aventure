using UnityEngine;
using static BaseStats;

public class SkullBullet : BulletBase
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
}
