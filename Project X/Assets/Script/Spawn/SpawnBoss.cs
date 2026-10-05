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
        GameObject objBopss = ObjectPooling.ObjectPooling_Instance.GetPool(KeyPool.KEY_BOSS_SLIMEKING);
        objBopss.transform.position = this.pointSpawn.position;
    }
}
