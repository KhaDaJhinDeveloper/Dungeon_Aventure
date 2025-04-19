using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponBase, IWeapon, IShoot
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
    }
    public void AllowTheAttack()
    {
        this.weaponSlotAttack.IsAttacking = !this.weaponSlotAttack.IsAttacking;
    }
    public void Shooting()
    {
        GameObject bulletAxe = ObjectPooling.ObjectPooling_Instance.GetPool(this.name + "Bullet");
        bulletAxe.transform.position = firePoint.position;
        bulletAxe.transform.rotation = firePoint.rotation;
        Rigidbody2D rbbullet = bulletAxe.GetComponent<Rigidbody2D>();
        rbbullet.velocity = transform.right * this.speedBullet;
    }
}
