using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float damagePhysicalRate;
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    protected virtual void DamageAttack(BaseStats target)
    {

    }
}
