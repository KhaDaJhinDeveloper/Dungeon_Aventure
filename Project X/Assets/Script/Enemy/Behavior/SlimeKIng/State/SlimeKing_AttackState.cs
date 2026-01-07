using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_AttackState : IState
{
    private SlimeKingController controller;
    public SlimeKing_AttackState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        DebugLogger.Log("Start Attack");
    }

    public void Execute()
    {
        DebugLogger.Log("Attack");
    }

    public void Exit()
    {
        DebugLogger.Log("End Attack");
    }
}
