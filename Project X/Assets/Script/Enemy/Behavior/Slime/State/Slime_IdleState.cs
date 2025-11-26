using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_IdleState : IState
{
    private SlimeController slimeController;
    public Slime_IdleState(SlimeController slimeController)
    {
        this.slimeController = slimeController;
    }
    public void Enter()
    {
        this.slimeController.LockVelocity();
    }

    public void Execute()
    {
        
    }
    public void Exit()
    {
        this.slimeController.UnLockVelocity();
    }
}
