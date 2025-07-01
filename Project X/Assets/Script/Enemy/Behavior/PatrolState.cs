using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : IState
{
    protected EnemyController enemyController;
    private float time;
    public PatrolState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }
    public void Enter()
    {
        float randomAngle = Random.Range(0f, 360f);
        this.enemyController.moveDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }
    public void Execute()
    {
        this.time += Time.deltaTime;
        if (IsWallDirection())
        {
            ChooseRandomDirection();
        }
        ChangeDirectionMove();
        this.enemyController.rb.velocity = this.enemyController.moveDirection * this.enemyController.baseStats.Speed;    
    }
    public void Exit()
    {
        
    }
    public bool IsWallDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(enemyController.transform.position, enemyController.moveDirection, enemyController.rayDistance, enemyController.nameLayer);
        return hit.collider != null;
    }
    public void ChangeDirectionMove()
    {
        if(this.time > 3)
        {
            float randomAngle = Random.Range(0f, 360f);
            this.enemyController.moveDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
            this.time = 0;
        }
    }
    void ChooseRandomDirection()
    {
        this.enemyController.moveDirection = -this.enemyController.moveDirection;
    }
}
