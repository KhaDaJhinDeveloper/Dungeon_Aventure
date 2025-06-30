using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBehavior : EnemyController
{
    private float coolDownTimer = 0;
    private GoblinStats goblinStats;
    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState(this));
        this.goblinStats = GetComponent<GoblinStats>();
    }
    protected override void Update()
    {
        base.Update();
        this.coolDownTimer += Time.deltaTime;
        this.CountTime();
        if( IsPlayerRangeChase(detectionRange))
        {
            if (IsPlayerRangeAttack(attackRange))
            {
                if (canAttack)
                {
                    ChangeState(new AttackState(this));
                    coolDownTimer = 0f;
                }
            }
            else ChangeState(new ChaseState(this));
        }    
        else
            ChangeState(new IdleState(this));
        if (this.goblinStats.IsDie)
            currentState = null;
        Flip();
    }
    public override void Flip()
    {
        if (transform.position.x > playerTransform.position.x)
            sr.flipX = true;
        else if (transform.position.x < playerTransform.position.x)
            sr.flipX = false;
    }
    void CountTime()
    {
        if(coolDownTimer > attackCoolDown)
        {
            canAttack = true;
        }          
        else canAttack = false;
    }    
}
