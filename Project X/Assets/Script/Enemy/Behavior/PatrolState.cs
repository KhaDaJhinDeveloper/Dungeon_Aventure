using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    protected EnemyController enemyController;
    public PatrolState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("this Stats Patrol");
    }
    public void Execute()
    {
        Debug.Log("this is Patrol");
    }
    public void Exit()
    {
        Debug.Log("Exit from Patrol");
    }
}
