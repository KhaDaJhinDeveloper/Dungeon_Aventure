using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDirection : MonoBehaviour
{
    [SerializeField] private Transform weaponPoint;
    public void UpdateWeaponDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f) return;

        Vector2 dir = direction.normalized;
        float rawAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(rawAngle / 45f) * 45f;
        dir = new Vector2(
            Mathf.Cos(snappedAngle * Mathf.Deg2Rad),
            Mathf.Sin(snappedAngle * Mathf.Deg2Rad)
        );

        float yRotate = dir.x < 0f ? 180f : 0f;
        float angle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;

        weaponPoint.localRotation = Quaternion.Euler(0, yRotate, angle);
    }
}
