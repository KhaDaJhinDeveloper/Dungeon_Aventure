using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private bool isChase;
    protected EnemyController enemyController;
    public ChaseState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }
    public void Enter()
    {
        this.isChase = true;
        
    }
    public void Execute()
    {
        if (this.isChase) Chase(); 
    }
    public void Exit()
    {
        this.isChase = false;
        this.enemyController.Ani.SetBool("run", false);
    }
    void Chase()
    {
        if (this.enemyController.playerTransform == null) return;
       CheckLineOfSight();
       if(this.enemyController.canSeeTarget)
       {
            this.enemyController.moveDirection = (this.enemyController.playerTransform.position - this.enemyController.transform.position).normalized;
            TryMove();
       }    
    }
    void CheckLineOfSight()
    {
        this.enemyController.canSeeTarget = false;
        float distanceTarget = Vector2.Distance(this.enemyController.playerTransform.position, this.enemyController.transform.position);
        if(this.enemyController.IsPlayerRangeChase(this.enemyController.DetectionRange))
        {
            Vector2 directionTarget = (this.enemyController.playerTransform.position - this.enemyController.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(this.enemyController.transform.position, directionTarget, distanceTarget, this.enemyController.nameLayer);
            if (hit.collider == null) this.enemyController.canSeeTarget = true;
        }    
    }

    void TryMove()
    {
        Vector2 currentPos = this.enemyController.transform.position;
        Vector2 desiredMove = this.enemyController.moveDirection * this.enemyController.baseStats.Speed* Time.deltaTime;
        Vector2 newPosition = currentPos + desiredMove;
        if (CanMoveTo(newPosition))
        {
            this.enemyController.transform.position = newPosition;
            this.enemyController.Ani.SetBool("run", true);
        }
    }
    bool CanMoveTo(Vector2 targetPosition)
    {
        Vector2 currentPos = this.enemyController.transform.position;
        Vector2 direction = (targetPosition - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, targetPosition);
        RaycastHit2D hit = Physics2D.CircleCast(currentPos, this.enemyController.enemySize, direction, distance, this.enemyController.nameLayer);
        return hit.collider == null;
    }

}
