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
        
    }
    public void Execute()
    {
        
    }
    public void Exit()
    {
        
    }
}
