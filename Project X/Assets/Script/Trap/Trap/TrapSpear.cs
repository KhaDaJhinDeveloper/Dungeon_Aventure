using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpear : BaseTrap
{
    [SerializeField] private LayerMask nameLayerTarget;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.damageType = new PhysicalDamage();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & nameLayerTarget.value) > 0)
        {
            BaseStats baseStats = collision.gameObject.GetComponentInChildren<BaseStats>();
            if (baseStats != null)
            {
                baseStats.TakeDamage(this.damage, this.damageType, this.transform);
            }
        }    
    }
}
