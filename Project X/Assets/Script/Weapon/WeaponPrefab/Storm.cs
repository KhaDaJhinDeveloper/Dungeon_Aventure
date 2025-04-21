using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStats;

public class Storm : WeaponBase, IWeapon
{
    private float time =0f;
    [SerializeField] float timeMax;
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
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
        {
            BaseStats objEnemy = collision.gameObject.GetComponentInChildren<BaseStats>();
            DealDamage(objEnemy);           
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
        {

        }
    }
    public override void DealDamage(BaseStats target)
    {
        target.TakeDamage(this.magical, this.damageType);
    }
}
