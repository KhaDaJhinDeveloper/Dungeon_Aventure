using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
[CreateAssetMenu(fileName = "MagicBallLegendShootPattern", menuName = "Weapon/WeaponAttack/ShootPattern/MagicBallLegend")]
public class MagicBallLegendShotPattern : ShootPattern
{
    int count = 0;
    public override void AttackShoot(WeaponInstance weapon, Transform firePoint)
    {
        if(count < 4)
        {
            SpawnBullet(weapon, firePoint, firePoint.rotation, this.bulletSpeed);
            this.count++;
            return;
        }
        GameObject bulletObj = ObjectPooling.ObjectPooling_Instance.GetPool(KeyPool.KEY_POOL_BULLET_MAGICBALL_LEGEND_SPECIAL);
        if (bulletObj != null)
        {
            bulletObj.transform.position = firePoint.position;
            bulletObj.transform.rotation = firePoint.rotation;
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            if (bullet != null)
                bullet.BulletLauch(weapon, this.bulletSpeed);
            this.count = 0;
            return;
        }
    }
}
