using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyStatsData
{
    public int maxHealth;
    public int currentHealth;
    public int armor;
    public int maxArmor;
    public int antimagic;
    public int maxAntimagic;
    public int speed;
}


[System.Serializable]
public class EnemyInstanceData
{
    public string nameEnemy;
    public Vector3 position;
    public EnemyStatsData enemyStatsData;
    public string uniqueID;
    public EnemyInstanceData() { }
    public EnemyInstanceData(string nameEnemy, Vector3 position, EnemyStatsData statsData, string uniqueID)
    {
        this.nameEnemy = nameEnemy;
        this.position = position;
        this.enemyStatsData = statsData;
        this.uniqueID = uniqueID;
    }
}

[System.Serializable]
public class EnemyData
{ 
    public List<EnemyInstanceData> enemyInstance = new List<EnemyInstanceData>();
}





