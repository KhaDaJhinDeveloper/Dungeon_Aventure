using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

public class Storm : WeaponBase, IWeapon
{
    protected override void Start()
    {
        this.damageType = new TrueDamage();
        base.Start();
    }
    public void WeaponAttack()
    {
        ani.SetTrigger("attack");
    }
    public void AllowTheAttack()
    {
        this.weaponSlotAttack.IsAttacking = !this.weaponSlotAttack.IsAttacking;
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
        {
            BaseStats objEnemy = collision.gameObject.GetComponentInChildren<BaseStats>();
            DealDamage(objEnemy);
        }
    }
    public override void DealDamage(BaseStats target)
    {
        target.TakeDamage(this.magical, this.damageType, this.transform);
    }
}
