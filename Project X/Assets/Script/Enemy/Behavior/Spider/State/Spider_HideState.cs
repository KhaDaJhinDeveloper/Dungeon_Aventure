using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_HideState : IState
{
    private SpiderController spiderController;
    public Spider_HideState(SpiderController spiderController)
    {
        this.spiderController = spiderController;
    }
    public void Enter()
    {
        this.spiderController.ani.SetBool("hide", true);
        this.spiderController.colli.enabled = false;
        this.spiderController.LockVelocity();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        this.spiderController.ani.SetBool("hide",false);
        this.spiderController.colli.enabled = true;
        this.spiderController.UnlockVelocity();
    }
}
