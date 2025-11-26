using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_IdleState : IState
{
    protected BatController batController;
    public Bat_IdleState(BatController batController)
    {
        this.batController = batController;
    }
    public void Enter()
    {
        
    }
    public void Execute()
    {
        this.batController.Rb.velocity = Vector2.zero;
    }
    public void Exit()
    {
        this.batController.Rb.velocity = this.batController.MoveDirection.normalized * this.batController.SpeedMove;
    }
}
