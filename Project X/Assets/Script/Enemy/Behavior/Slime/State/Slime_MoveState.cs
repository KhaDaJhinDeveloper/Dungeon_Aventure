using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_MoveState : IState
{
    private SlimeController slimeController;
    public Slime_MoveState(SlimeController slimeController)
    {
        this.slimeController = slimeController;
    }
    public void Enter()
    {
        this.slimeController.Ani.SetBool("move", true);
        ChangeMoveDirection();
    }

    public void Execute()
    {
        Move();
    }

    public void Exit()
    {
        this.slimeController.Ani.SetBool("move", false);
    }
    private void Move()
    {
        if (IsWall())
            ChooseRandomDirection();
        this.slimeController.UnLockVelocity();
    }
    private bool IsWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.slimeController.WallCheck.position, this.slimeController.MoveDirection , this.slimeController.RayDistance, this.slimeController.WallLayer);
        return hit.collider != null;
    }
    private void ChangeMoveDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        this.slimeController.MoveDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }
    void ChooseRandomDirection()
    {
        this.slimeController.MoveDirection = -this.slimeController.MoveDirection;
    }
}
