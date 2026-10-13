using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyStatsData
{
    public int currentHealth;
    public int maxHealth;
    public int armor;
    public int antiMagic;
    public int maxArmor;
    public int maxAntiMagic;
    //public int speed;
    public int damage;
    public EnemyStatsData(int maxHealth,int maxArmor,  int maxAntiMagic, int currentHealth, int armor, int antiMagic, int damage)
    {

        this.maxHealth = maxHealth;
        this.maxArmor = maxArmor;
        this.maxAntiMagic = maxAntiMagic;
        this.currentHealth = currentHealth;
        this.armor = armor;
        this.antiMagic = antiMagic;
        //this.speed = speed;
        this.damage = damage;
    }
}

[System.Serializable]
public class EnemyInstanceData
{
    public int enemyId;
    public Vector3 position;
    public EnemyStatsData stats;
    public EnemyInstanceData(int enemyType, Vector3 position, EnemyStatsData stats)
    {
        this.enemyId = enemyType;
        this.position = position;
        this.stats = stats;
    }
}

[System.Serializable]
public class AllEnemyDataSerializable
{
    public string sceneName;
    public List<EnemyInstanceData> allEnemies = new List<EnemyInstanceData>();
}






