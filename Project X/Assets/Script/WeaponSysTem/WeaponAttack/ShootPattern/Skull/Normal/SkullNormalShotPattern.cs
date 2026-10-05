using UnityEngine;
[CreateAssetMenu(fileName = "SkullNormalShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/SkullNormal")]
public class SkullNormalShotPattern : ShootPattern
{
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
        SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
    }
}
