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
        Debug.Log("Start IdleState");
    }

    public void Execute()
    {
        Debug.Log("IdleState");
        this.batController.Rb.velocity = Vector2.zero;
    }

    public void Exit()
    {
        Debug.Log("End IDleState");
        this.batController.Rb.velocity = this.batController.MoveDirection.normalized * this.batController.SpeedMove;
    }
}
