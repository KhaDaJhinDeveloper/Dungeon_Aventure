using UnityEngine;
[CreateAssetMenu(fileName = "SkullLegendShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/SkullLegend")]
public class SkullLegendShotPattern : ShootPattern
{
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
            SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
    }
}
