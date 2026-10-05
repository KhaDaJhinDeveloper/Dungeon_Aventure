using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : WeaponBase, IWeapon, IShoot
{
    [SerializeField] int speedBullet;
    Transform firePoint;
    protected override void Start()
    {
        base.Start();
        this.firePoint = this.transform;
    }
    public void WeaponAttack()
    {
        ani.SetTrigger("attack");
        Shooting();
    }
    public void AllowTheAttack()
    {
    }
    public void Shooting()
    {
        GameObject bulletSkull = ObjectPooling.ObjectPooling_Instance.GetPool(KeyPool.KEY_POOL_BULLET_SKULL);
        bulletSkull.transform.position = firePoint.position;
        bulletSkull.transform.rotation = firePoint.rotation;
        Rigidbody2D rbbullet = bulletSkull.GetComponent<Rigidbody2D>();
        rbbullet.velocity = transform.right * this.speedBullet;
    }
}
