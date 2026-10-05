
using UnityEngine;
[System.Serializable]
public class PlayerWeaponSlotData
{
    public WeaponSaveData weaponSlotLeft;
    public WeaponSaveData weaponSlotRight;
    public WeaponSaveData weaponReserve;
}


[System.Serializable]
public class WeaponSaveData
{
    public string weaponID;
    public int mainDamage;
    public WeaponSaveData() { }
    public WeaponSaveData(string weaponID, int mainDamage)
    {
        this.weaponID = weaponID;
        this.mainDamage = mainDamage;
    }
}
