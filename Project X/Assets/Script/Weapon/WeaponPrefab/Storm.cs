using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

public class Storm : WeaponBase, IWeapon
{
    private IDamageType damageType;
    protected override void Start()
    {
        this.damageType = new MagicalDamage();
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
    public override void DealDamage(BaseStats target)
    {
        target.TakeDamage(strength, damageType);
    }
}
