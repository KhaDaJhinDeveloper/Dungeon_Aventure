using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public KeyPool keyPool;
    [Header("Stats Weapon")]
    public WeaponType weaponType;
    public string weaponName;
    public int physicDamage ;
    public int magicDamage ;
    public int trueDamage ;
    public int mixDamage;
    public bool isUsedMana;
    public int manaQty;
    [Range(0f, 1f)]public float rateMixDamage = 0.5f;
    [Header("Ranger Weapon")]
    public ShootPattern shootPattern;
    public KeyPool keyBullet;
    [Header("Visual Weapon")]
    public Sprite weaponSprite;
    public RuntimeAnimatorController animatorController;
}
