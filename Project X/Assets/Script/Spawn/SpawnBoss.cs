using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : SpawnBase
{
    [SerializeField] private Transform pointSpawn;
    protected override void Start()
    {
        base.Start();
        if (EnemiesDataManager.Instance.HasData())
            return;
        else
            Spawn();
    }
    protected override void Spawn()
    {
        string key = KeyClean.CleanKey(this.prefab[0].name);
        GameObject objBopss = ObjectPooling.ObjectPooling_Instance.GetPool(key);
        objBopss.transform.position = this.pointSpawn.position;
    }
}
