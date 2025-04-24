using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    protected EnemyController enemyController;
    public IdleState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("this Stats Idle");
    }
    public void Execute()
    {
        Debug.Log("this is Idle");
    }
    public void Exit()
    {
        Debug.Log("Exit from Idle");
    }
}
