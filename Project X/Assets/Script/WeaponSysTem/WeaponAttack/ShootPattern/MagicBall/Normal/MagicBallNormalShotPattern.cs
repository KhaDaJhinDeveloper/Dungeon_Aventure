using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MagicBallNormalShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/MagicBallNormal")]
public class MagicBallNormalShotPattern : ShootPattern
{
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
        SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
    }
}
