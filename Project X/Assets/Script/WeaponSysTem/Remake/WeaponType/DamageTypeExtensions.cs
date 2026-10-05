using UnityEngine;
[System.Serializable]
public enum WeaponType { Physic, Magic, Mixed, True}
public static class DamageTypeExtensions
{
    public static IDamageType GetOppositeDamageType(this WeaponData data)
    {
        switch (data.weaponType)
        {
            case WeaponType.Physic:
                return new PhysicalDamage();
            case WeaponType.Magic:
                return new MagicalDamage();
            case WeaponType.Mixed:
                return new MixedDamage(data.rateMixDamage);
            case WeaponType.True:
                return new TrueDamage();
            default: return null;
        }
    }
    public static int GetOppositeMainDamage(this WeaponInstance data)
    { 
        switch(data.damageType)
        {
            case PhysicalDamage:
                return data.physicDamage;
            case MagicalDamage:
                return data.magicDamage;
            case TrueDamage:
                return data.trueDamage;
            case MixedDamage:
                return data.mixDamage;
            default: return data.trueDamage;
        }
    }
}