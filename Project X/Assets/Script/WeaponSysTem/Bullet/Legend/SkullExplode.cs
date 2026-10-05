using UnityEngine;

public class SkullExplode : BulletBase
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
        }
    }
    protected override void DamageAttack(BaseStats target)
    {
        target.TakeDamage(this.damageMain, this.damageType, this.transform);
    }
    public void ExplodeLaunch(int damageMain, IDamageType damageType, float ratemixDamage, Transform pos)
    { 
        this.damageMain = damageMain;
        this.damageType = damageType;
        this.rateMixDamage = ratemixDamage;
        this.transform.position = pos.position;
    }
    public void HideExplode()
    {
        ObjectPooling.Instance.ReturnToPool(KeyPool.KEY_POOL_BULLET_EXPLODESKULL, this.gameObject);
    }
}
