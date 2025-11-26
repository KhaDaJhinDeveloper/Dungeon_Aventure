using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime_SpawnState : IState
{
    private SlimeController slimeController;
    private int quantity;
    public Slime_SpawnState(SlimeController slimeController, int quantity)
    {
        this.slimeController = slimeController;
        this.quantity = quantity;
    }
    public void Enter()
    {
        SpawnChild();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
    void SpawnChild()
    {
        for(int i = 0; i < quantity; i++)
        {
            GameObject slimeChild = ObjectPooling.ObjectPooling_Instance.GetPool("SlimeChild");
            slimeChild.transform.position = RandomPosDrop(this.slimeController.transform.position);
        }
    }
    Vector2 RandomPosDrop(Vector2 pos)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 dropPosition = pos + randomDirection * Random.Range(0.5f, 0.8f);
        return dropPosition;
    }
}
