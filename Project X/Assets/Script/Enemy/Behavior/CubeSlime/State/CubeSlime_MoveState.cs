using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlime_MoveState : IState
{
    private CubeSlime_Controller cubeSlimeController;
    public CubeSlime_MoveState(CubeSlime_Controller slimeController)
    {
        this.cubeSlimeController = slimeController;
    }
    public void Enter()
    {
        this.cubeSlimeController.Ani.SetBool("move", true);
        ChangeMoveDirection();
    }

    public void Execute()
    {
        Move();
    }

    public void Exit()
    {
        this.cubeSlimeController.Ani.SetBool("move", false);
    }
    private void Move()
    {
        if (IsWall())
            ChooseRandomDirection();
        this.cubeSlimeController.UnLockVelocity();
    }
    private bool IsWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.cubeSlimeController.WallCheck.position, this.cubeSlimeController.MoveDirection, this.cubeSlimeController.RayDistance, this.cubeSlimeController.WallLayer);
        return hit.collider != null;
    }
    private void ChangeMoveDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        this.cubeSlimeController.MoveDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }
    void ChooseRandomDirection()
    {
        this.cubeSlimeController.MoveDirection = -this.cubeSlimeController.MoveDirection;
    }
}
