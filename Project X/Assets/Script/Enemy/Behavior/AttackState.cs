using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackState : IState
{
    protected EnemyController enemyController;
    private float coolDownTimer = 0;
    public AttackState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }
    public void Enter()
    {
        this.enemyController.canAttack = true;
    }
    public void Execute()
    {
        if (this.enemyController.canAttack) Attack();
    }
    public void Exit()
    {
        this.enemyController.Ani.SetBool("attack", false);
    }
    void Attack()
    {
        this.enemyController.Ani.SetBool("attack",true);
    }
}
