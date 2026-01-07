using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_IdleState : IState
{
    private SlimeKingController controller;
    public SlimeKing_IdleState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        this.controller.LockVelocity();
    }

    public void Execute()
    {
        DebugLogger.Log("Idle");
    }

    public void Exit()
    {
        this.controller.UnLockVelocity();
    }
}
