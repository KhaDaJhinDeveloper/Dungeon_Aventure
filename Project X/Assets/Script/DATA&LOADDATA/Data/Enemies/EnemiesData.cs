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
    public int speed;
    public EnemyStatsData(int currentHealth,int maxHealth, int armor, int antiMagic, int maxArmor, int maxAntiMagic, int speed)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.armor = armor;
        this.antiMagic = antiMagic;
        this.maxArmor = maxArmor;
        this.maxAntiMagic = maxAntiMagic;
        this.speed = speed;
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
public class SceneEnemyData
{
    public string sceneName;
    public List<EnemyInstanceData> enemies = new List<EnemyInstanceData>();
    public SceneEnemyData(string sceneName, List<EnemyInstanceData> enemies)
    {
        this.sceneName = sceneName;
        this.enemies = enemies;
    }
}

[System.Serializable]
public class AllEnemyDataSerializable
{
    public List<SceneEnemyData> sceneEnemies = new List<SceneEnemyData>();
}





