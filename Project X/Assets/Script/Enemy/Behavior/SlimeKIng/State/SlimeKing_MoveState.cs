using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_MoveState : IState
{
    private SlimeKingController controller;
    public SlimeKing_MoveState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        DebugLogger.Log("Start Move");
    }

    public void Execute()
    {
        DebugLogger.Log("Move");
    }

    public void Exit()
    {
        DebugLogger.Log("End Move");
    }
}
