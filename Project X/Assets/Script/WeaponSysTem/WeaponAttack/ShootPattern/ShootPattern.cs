using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapon/WeaponAttack/WeaponShot")]
public abstract class ShootPattern : ScriptableObject
{
    [SerializeField] protected float bulletSpeed;
    public abstract void AttackShoot(WeaponInstance weapon, Transform firePoint);
    protected virtual void SpawnBullet(WeaponInstance weapon, Transform firePoint, Quaternion rotation, float speed)
    {
        GameObject bulletObj = ObjectPooling.ObjectPooling_Instance.GetPool(weapon.keyBullet);
        if (bulletObj != null)
        {
            bulletObj.transform.position = firePoint.position;
            bulletObj.transform.rotation = rotation;
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            if (bullet != null)
                bullet.BulletLauch(weapon, speed);
        }
    }
}
