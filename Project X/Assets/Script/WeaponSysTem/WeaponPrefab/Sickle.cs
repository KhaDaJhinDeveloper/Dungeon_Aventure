using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sickle : WeaponBase, IWeapon
{
    protected override void Start()
    {
        base.Start();
        this.damageType = new PhysicalDamage();
    }
    public void WeaponAttack()
    {
        ani.SetTrigger("attack");
    }
    public void AllowTheAttack()
    {
        
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
        {
            BaseStats objEnemy = collision.gameObject.GetComponentInChildren<BaseStats>();
            DealDamage(objEnemy);
        }
    }
    public override void DealDamage(BaseStats target)
    {
        target.TakeDamage(this.strength, this.damageType,this.transform);
    }
}
