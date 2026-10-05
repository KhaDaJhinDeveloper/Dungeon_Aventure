using UnityEngine;

[System.Serializable]
public class PlayerStatsData 
{
    public int maxHeal;
    public int currentHeal;
    public int speed;
    public int armor;
    public int antiMagic;
    public int mana;
    public int maxArmor;
    public int maxAntiMagic;
    public int maxMana;
    public PlayerStatsData() { }
    public PlayerStatsData(int maxHeal, int currentHeal, int speed, int armor, int antiMagic,int mana, int maxArmor, int maxAntiMagic, int maxMana)
    {
        this.maxHeal = maxHeal;
        this.currentHeal = currentHeal;
        this.speed = speed;
        this.armor = armor;
        this.antiMagic = antiMagic;
        this.mana = mana;
        this.maxArmor = maxArmor;
        this.maxAntiMagic = maxAntiMagic;
        this.maxMana = maxMana;
    }
}

[System.Serializable]
public class PlayerPositionData
{
    public Vector3 position;
    public PlayerPositionData() { }
    public PlayerPositionData(Vector3 position)
    {
        this.position = position;
    }
}


[System.Serializable]
public class WeaponsData
{
    public string weaponName1;
    public string weaponName2;
    public string weaponReserve;
    public WeaponsData() { }
    public WeaponsData(string weaponName1, string weaponName2, string weaponReserve)
    {
        this.weaponName1 = weaponName1;
        this.weaponName2 = weaponName2;
        this.weaponReserve = weaponReserve;
    }
}