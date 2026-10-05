using UnityEngine;
[CreateAssetMenu(fileName = "BowNormalShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/BowNormal")]
public class BowNormalShot : ShootPattern
{
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
        SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
    }
}
