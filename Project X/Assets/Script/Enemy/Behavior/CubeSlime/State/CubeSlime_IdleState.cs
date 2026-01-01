using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlime_IdleState : IState
{
    private CubeSlime_Controller cubeSlimeController;
    public CubeSlime_IdleState(CubeSlime_Controller controller)
    {
        this.cubeSlimeController = controller;
    }
    public void Enter()
    {
        this.cubeSlimeController.LockVelocity();
    }

    public void Execute()
    {

    }
    public void Exit()
    {
        this.cubeSlimeController.UnLockVelocity();
    }
}
