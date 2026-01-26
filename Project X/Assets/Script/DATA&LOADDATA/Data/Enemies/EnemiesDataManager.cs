using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDataManager : Singleton<EnemiesDataManager>,IDataManager
{
    #region FILE_NAME
    private const string FILE_ENEMY_DATA = "EnemiesData.json";
    #endregion
    private List<EnemyData> sceneEnemies = new List<EnemyData>();
    protected override void Awake()
    {
        base.Awake();
    }
    public void SaveData()
    {
        this.sceneEnemies = new List<EnemyData>();
        foreach(EnemyData keyEnemies in this.sceneEnemies)
        {
            ///keyEnemies = this.sceneEnemies.Add(new EnemyInstanceData());
        }
    }
    public void LoadData()
    {
        throw new System.NotImplementedException();
    }
    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }
}
