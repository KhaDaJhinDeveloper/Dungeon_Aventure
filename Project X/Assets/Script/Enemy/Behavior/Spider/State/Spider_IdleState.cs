using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IdleState : IState
{
    private SpiderController spiderController;
    public Spider_IdleState(SpiderController spiderController) 
    {
        this.spiderController = spiderController;
    }
    public void Enter()
    {
        this.spiderController.ani.SetBool("idle", true);
        this.spiderController.LockVelocity();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        this.spiderController.ani.SetBool("idle", false);
        this.spiderController.UnlockVelocity();
    }
}
