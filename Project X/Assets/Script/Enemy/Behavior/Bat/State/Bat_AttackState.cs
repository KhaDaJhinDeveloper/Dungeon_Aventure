using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_AttackState : IState
{
    protected BatController batController;
    public Bat_AttackState(BatController batController)
    {
        this.batController = batController;
    }
    public void Enter()
    {
        Debug.Log("Start Attack");
    }

    public void Execute()
    {
        Debug.Log("Attack");
    }

    public void Exit()
    {
        Debug.Log("End Attack");
    }
}
