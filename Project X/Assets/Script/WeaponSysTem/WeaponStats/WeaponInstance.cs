using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance 
{
    public WeaponData baseData;
    public string nameWeapon;
    public int physicDamage;
    public int magicDamage;
    public int trueDamage;
    public int mixDamage;
    public int mainDamage;
    public IDamageType damageType;
    public Sprite spriteWeapon;
    public KeyPool keyBullet;
    public bool isUsedMana;
    public int manaQty;
    [Range(0f, 1f)] public float rateMixDamage;
    public WeaponInstance(WeaponData data)
    {
        this.spriteWeapon = data.weaponSprite;
        this.baseData = data;
        this.nameWeapon = data.weaponName + " " + (GameControl.Instance.LevelOfDanger >= 1 ? GameControl.Instance.LevelOfDanger.ToString() : "");
        this.physicDamage = data.physicDamage + Mathf.RoundToInt(data.physicDamage * GameControl.Instance.ScaleDamage);
        this.magicDamage = data.magicDamage + Mathf.RoundToInt(data.magicDamage * GameControl.Instance.ScaleDamage);
        this.trueDamage = data.trueDamage + Mathf.RoundToInt(data.trueDamage * GameControl.Instance.ScaleDamage);
        this.mixDamage = data.mixDamage + Mathf.RoundToInt(data.mixDamage * GameControl.Instance.ScaleDamage);
        this.rateMixDamage = data.rateMixDamage;
        this.damageType = data.GetOppositeDamageType();
        this.keyBullet = data.keyBullet;
        this.mainDamage = this.GetOppositeMainDamage();
        this.isUsedMana = data.isUsedMana;
        this.manaQty = data.manaQty;
    }
    #region Save&&Load
    public WeaponInstance(WeaponData data, WeaponSaveData saveData ) : this(data) 
    {
        this.nameWeapon = saveData.weaponID;
        this.mainDamage = saveData.mainDamage;
    }
    public WeaponSaveData ToSaveData()
    {
        if(this.baseData == null) return null;
        return new WeaponSaveData(this.baseData.weaponName, this.mainDamage);
    }
    #endregion
}
