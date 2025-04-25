using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    protected EnemyController enemyController;
    public AttackState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("this Stats Attack");
    }
    public void Execute()
    {
        Debug.Log("this is Attack");
    }
    public void Exit()
    {
        Debug.Log("Exit from Attack");
    }
}
