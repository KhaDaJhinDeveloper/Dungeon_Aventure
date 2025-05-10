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
       
    }
    public void Execute()
    {       
        Chase();  
        enemyController.Flip();
    }
    public void Exit()
    {

    }
    public void Chase()
    {
        enemyController.transform.position = Vector2.MoveTowards(enemyController.transform.position, enemyController.playerTransform.position, 2 * Time.deltaTime);
    }
}
