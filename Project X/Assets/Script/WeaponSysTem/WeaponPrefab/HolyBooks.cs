using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBooks : WeaponBase, IWeapon
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
        
    }
    public override void DealDamage(BaseStats target)
    {
        
    }
}
