using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatStats : BaseStats
{
    [SerializeField] int damage;
    IDamageType damageType;
    protected override void Start()
    {
        base.Start();
        this.damageType = new PhysicalDamage();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            BaseStats objPlayer = collision.gameObject.GetComponentInChildren<BaseStats>();
            objPlayer.TakeDamage(this.damage, this.damageType);
        }
    }
    protected override void Die()
    {
        base.Die();
    }
}
