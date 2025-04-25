using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChaseState : IState
{
    protected EnemyController enemyController;
    public ChaseState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }
    public void Enter()
    {
        Debug.Log("this Stats Chase");
    }
    public void Execute()
    {
        Debug.Log("this is Chase");
        Chase();       
    }
    public void Exit()
    {
        Debug.Log("Exit from Chase");
    }
    public void Chase()
    {
        enemyController.transform.position = Vector2.MoveTowards(enemyController.transform.position, enemyController.playerTransform.position, 2 * Time.deltaTime);
    }
}
