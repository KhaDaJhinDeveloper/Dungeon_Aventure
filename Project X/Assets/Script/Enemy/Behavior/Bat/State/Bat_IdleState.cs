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
    }

    public void Exit()
    {
        Debug.Log("End IDleState");
    }
}
