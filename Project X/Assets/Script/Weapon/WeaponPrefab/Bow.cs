using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponBase, IWeapon, IShoot
{
    protected override void Start()
    {
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
    public void Shooting()
    {

    }
}
