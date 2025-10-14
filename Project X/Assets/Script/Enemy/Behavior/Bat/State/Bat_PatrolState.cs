using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_PatrolState : IState
{
    protected BatController batController;
    public Bat_PatrolState(BatController batController) 
    { 
        this.batController = batController;
    }
    public void Enter()
    {
        Debug.Log("Start Patrol");
    }

    public void Execute()
    {
        Debug.Log("Patrol");
    }

    public void Exit()
    {
        Debug.Log("End Patrol");
    }
}
