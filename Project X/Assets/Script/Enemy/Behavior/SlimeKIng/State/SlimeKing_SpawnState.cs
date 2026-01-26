using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_SpawnState : IState
{
    private SlimeKingController controller;
    private int maxQuantity;
    private int minQuantity;
    private float maxTimeSpawn;
    private float currentTime;
    public SlimeKing_SpawnState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        this.controller.LockVelocity();
        this.maxQuantity = this.controller.MaxQuantity;
        this.maxTimeSpawn = this.controller.MaxTimeSpawn;
        this.minQuantity = 0;
    }

    public void Execute()
    {       
        SpawnChild();
    }

    public void Exit()
    {
        this.controller.UnLockVelocity();
    }
    void SpawnChild()
    {
        if (this.minQuantity >= this.maxQuantity) return;

        this.currentTime += Time.deltaTime;

        if (currentTime >= this.maxTimeSpawn)
        {
            GameObject slime = ObjectPooling.ObjectPooling_Instance.GetPool("Slime");
            slime.transform.position = RandomPosDrop(this.controller.transform.position);

            this.currentTime = 0f;
            this.minQuantity++;
        }
    }
    Vector2 RandomPosDrop(Vector2 pos)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 dropPosition = pos + randomDirection * Random.Range(0.5f, 0.8f);
        return dropPosition;
    }
}
