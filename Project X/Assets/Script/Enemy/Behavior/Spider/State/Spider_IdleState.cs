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
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
