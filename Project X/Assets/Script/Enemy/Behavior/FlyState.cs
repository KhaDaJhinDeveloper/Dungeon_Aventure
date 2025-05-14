using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FlyState : IState
{
    protected EnemyController enemyController;
    public FlyState(EnemyController enemyController)
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
