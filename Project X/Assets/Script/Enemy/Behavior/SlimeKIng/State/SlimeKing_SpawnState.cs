using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_SpawnState : IState
{
    private SlimeKingController controller;
    public SlimeKing_SpawnState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        DebugLogger.Log("Start Spawn");
    }

    public void Execute()
    {
        DebugLogger.Log("Spawn");
    }

    public void Exit()
    {
        DebugLogger.Log("End Spawn");
    }
}
