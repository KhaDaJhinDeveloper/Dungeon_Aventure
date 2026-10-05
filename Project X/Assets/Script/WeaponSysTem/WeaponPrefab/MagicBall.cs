using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : WeaponBase, IWeapon, IShoot
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
        GameObject bulletMagic = ObjectPooling.ObjectPooling_Instance.GetPool(KeyPool.KEY_POOL_BULLET_MAGICBALL);
        bulletMagic.transform.position = firePoint.position;
        bulletMagic.transform.rotation = firePoint.rotation;
        Rigidbody2D rbbullet = bulletMagic.GetComponent<Rigidbody2D>();
        rbbullet.velocity = transform.right * this.speedBullet;
    }
}
