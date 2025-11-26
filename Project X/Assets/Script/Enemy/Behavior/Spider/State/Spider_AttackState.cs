using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_AttackState : IState
{
    private float currentTime;
    private SpiderController spiderController;
    public Spider_AttackState(SpiderController spiderController)
    {
        this.spiderController = spiderController;
    }
    public void Enter()
    {
        this.spiderController.ani.SetBool("attack", true);
        this.spiderController.LockVelocity();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        this.spiderController.ani.SetBool("attack", false);
    }
}
