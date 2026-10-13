using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesDataManager : Singleton<EnemiesDataManager>, IDataManager
{
    #region FILE_NAME
    private const string FILE_ENEMY_DATA = "EnemiesData.json";
    private AllEnemyDataSerializable allData ;
    #endregion
    protected override void Awake()
    {
        base.Awake();
    }
    public void SaveData()
    {
        this.allData = new AllEnemyDataSerializable();
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag(TagManager.TAG_ENEMY);
        if (enemyList.Length <= 0) return;
        this.allData.sceneName = SceneExtensions.GetCurrentSceneName();
        foreach(var obj in enemyList)
        {
            EnemyStatsData statsData = GetStatsData(obj);
            EnemyInstanceData instanceData = GetEnemyData(obj, statsData);
            this.allData.allEnemies.Add(instanceData);
        }
        JsonFileUtility.SaveToJson<AllEnemyDataSerializable>(this.allData, FILE_ENEMY_DATA);
    }
    public void LoadData()
    {
        this.allData = JsonFileUtility.LoadFromJson<AllEnemyDataSerializable>(FILE_ENEMY_DATA);
        if(this.allData == null) return;
        foreach(var data in  this.allData.allEnemies)
        {
            GameObject enemy = ObjectPooling.Instance.GetPool((KeyPool)data.enemyId);
            if(enemy == null) continue;
            enemy.transform.position = data.position;
            if (enemy.TryGetComponent<BaseStats>(out BaseStats stats))
                stats?.ScaleStats(GameControl.Instance.ScaleDamage, data.stats);
        }
        DeleteData();
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_ENEMY_DATA);
        this.allData = new();
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_ENEMY_DATA);
    }
    public bool HasMapData(string mapName)
    {
        if (!HasData()) return false;
        AllEnemyDataSerializable data = JsonFileUtility.LoadFromJson<AllEnemyDataSerializable>(FILE_ENEMY_DATA);
        if (string.IsNullOrEmpty(data.sceneName))
            return false;
        else return data.sceneName == mapName;
    }
    public EnemyStatsData GetStatsData(GameObject enemy)
    {
        BaseStats stats = enemy.GetComponent<BaseStats>();
        if (stats == null) return null;
        EnemyStatsData data = new(  stats.MaxHealth,
                                    stats.MaxArmor,
                                    stats.MaxAntiMagic,
                                    stats.CurentHealth,
                                    stats.Armor,
                                    stats.AntiMagic,
                                    //stats.Speed,
                                    stats.Damage);
        return data;
    }
    public EnemyInstanceData GetEnemyData(GameObject enemy, EnemyStatsData statsData)
    {
        EnemyIDTracker enemyID = enemy.GetComponent<EnemyIDTracker>();
        if (enemyID == null) return null;
        Vector3 pos = enemy.transform.position;
        int ID = (int)enemyID.uniqueID;
        EnemyInstanceData instanceData= new(ID, pos, statsData);
        return instanceData;
    }
}
