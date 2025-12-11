using UnityEngine;

[System.Serializable]
public class PlayerStatsData 
{
    public int maxHeal;
    public int currentHeal;
    public int speed;
    public int armor;
    public int antiMagic;
    public int damage;
    public PlayerStatsData() { }
    public PlayerStatsData(int maxHeal, int currentHeal, int speed, int armor, int antiMagic, int damage = 0)
    {
        this.maxHeal = maxHeal;
        this.currentHeal = currentHeal;
        this.speed = speed;
        this.armor = armor;
        this.antiMagic = antiMagic;
        this.damage = damage;
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