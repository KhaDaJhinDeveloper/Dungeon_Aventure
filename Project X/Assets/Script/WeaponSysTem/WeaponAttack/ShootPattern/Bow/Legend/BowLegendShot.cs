using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BowLegendShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/BowLegend")]
public class BowLegendShot : ShootPattern
{
    [SerializeField] private int bulletQuantity;
    [SerializeField] private float bulletAngle;
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
        if(this.bulletQuantity <= 1)
        {
            SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
            return;
        }
        float startAngle = -this.bulletAngle / 2;
        float angleStep = this.bulletAngle / (this.bulletQuantity - 1);
        for (int i = 0; i < this.bulletQuantity; i++)
        {
            float currentOffset = startAngle + (angleStep * i);
            Quaternion rotatin = firePoint.rotation * Quaternion.Euler(0, 0, currentOffset);
            SpawnBullet(weapon, firePoint, rotatin, this.bulletSpeed);
        }
    }
}
