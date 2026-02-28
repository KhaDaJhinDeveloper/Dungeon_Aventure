using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesDataManager : Singleton<EnemiesDataManager>,IDataManager
{
    #region FILE_NAME
    private const string FILE_ENEMY_DATA = "EnemiesData.json";
    private Dictionary<string, List<EnemyInstanceData>> sceneEnemies = new Dictionary<string, List<EnemyInstanceData>>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    #endregion
    protected override void Awake()
    {
        base.Awake();
    }
    public void SaveData()
    {
        DeleteData();
        AllEnemyDataSerializable allEnemyDataSerializable = new AllEnemyDataSerializable();
        List<EnemyInstanceData> allEnemies = new List<EnemyInstanceData>();
        string currentScene = SceneManager.GetActiveScene().name; 
        List<EnemyInstanceData> enemydata = GetEnemiesData();
        allEnemies.AddRange(enemydata);           
        this.sceneEnemies[currentScene] = allEnemies;
        foreach (KeyValuePair<string, List<EnemyInstanceData>> data in sceneEnemies)
        {
            allEnemyDataSerializable.sceneEnemies.Add(new SceneEnemyData(data.Key, data.Value));
        }
        JsonFileUtility.SaveToJson(allEnemyDataSerializable, FILE_ENEMY_DATA);
    }
    public void LoadData()
    {
        //Not use
    }
    public void LoadData(string Scenename)
    {
        int count = 0;
        AllEnemyDataSerializable allData = JsonFileUtility.LoadFromJson<AllEnemyDataSerializable>(FILE_ENEMY_DATA);
        if (allData == null) return;
        foreach (SceneEnemyData sceneData in allData.sceneEnemies)
        {
            if (sceneData.sceneName == Scenename)
            {
                this.sceneEnemies[sceneData.sceneName] = sceneData.enemies;
                foreach (EnemyInstanceData data in this.sceneEnemies[sceneData.sceneName])
                {
                    count++;
                    GameObject enemy = ObjectPooling.ObjectPooling_Instance.GetPool(data.enemyType);
                    if (enemy == null) continue;
                    enemy.transform.position = data.position;
                    BaseStats stats = enemy.GetComponent<BaseStats>();
                    EnemyIDTracker iDTracker = enemy.GetComponent<EnemyIDTracker>();
                    if (stats != null && iDTracker != null)
                    {
                        StartCoroutine(DelayLoad(stats, iDTracker, data));
                    }
                }
                DebugLogger.Log(this.sceneEnemies[sceneData.sceneName].Count);
                break;
                
            }
        }
        DebugLogger.Log(count);
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_ENEMY_DATA);
        this.sceneEnemies = new Dictionary<string, List<EnemyInstanceData>>();
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_ENEMY_DATA);
    }
    public List<GameObject> GetEnemiesList()
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag(TagManager.TAG_ENEMY);
        foreach(GameObject enemy in enemyList)
        {
            if (!enemy.activeInHierarchy || enemy == null) 
                continue;
            else list.Add(enemy);
        }
        return list; 
    }
    public List<EnemyInstanceData> GetEnemiesData()
    {
        List<EnemyInstanceData> enemyInstanceDatas = new List<EnemyInstanceData>();
        this.spawnedEnemies = GetEnemiesList().ToList();
        foreach (GameObject enemy in this.spawnedEnemies)
        {
            if (enemy == null || !enemy.activeInHierarchy) continue;
            string nameEnemy = KeyClean.CleanKey(enemy.name);
            BaseStats stats = enemy.GetComponent<BaseStats>();
            EnemyIDTracker idTracker = enemy.GetComponent<EnemyIDTracker>();
            if (stats != null && idTracker != null)
            {
                EnemyStatsData enemyStats = new EnemyStatsData
                (
                    stats.CurentHealth,
                    stats.MaxHealth,
                    stats.Armor,
                    stats.AntiMagic,
                    stats.MaxArmor,
                    stats.MaxAntiMagic,
                    stats.Speed
                );
                EnemyInstanceData enemyIntance = new EnemyInstanceData
                (
                    nameEnemy,
                    enemy.transform.position,
                    enemyStats,
                    idTracker.uniqueID
                );
                enemyInstanceDatas.Add(enemyIntance);
            }
        }
        return enemyInstanceDatas;
    }
    IEnumerator DelayLoad(BaseStats stats, EnemyIDTracker iDTracker, EnemyInstanceData data)
    {
        yield return null;
        stats.CurentHealth = data.stats.currentHealth;
        stats.MaxHealth = data.stats.maxHealth;
        stats.Armor = data.stats.armor;
        stats.AntiMagic = data.stats.antiMagic;
        stats.MaxArmor = data.stats.maxArmor;
        stats.MaxAntiMagic = data.stats.maxAntiMagic;
        stats.Speed = data.stats.speed;
        iDTracker.uniqueID = data.uniqueId;
        stats.UpdateUI();
    }
}
