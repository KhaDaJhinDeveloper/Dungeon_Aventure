using UnityEngine;

public class SkullBulletLegend : BulletBase
{
    [SerializeField] private LayerMask nameLayerTarget;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & nameLayerTarget.value) > 0)
        {
            Explode(this.transform);
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
    void Explode(Transform pos)
    {
        SkullExplode explode = ObjectPooling.Instance.GetPool(KeyPool.KEY_POOL_BULLET_EXPLODESKULL)?.GetComponent<SkullExplode>();
        explode?.ExplodeLaunch(this.damageMain, this.damageType, this.rateMixDamage,this.transform);
    }
}
