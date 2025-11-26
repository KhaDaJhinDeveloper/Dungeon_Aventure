using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_MoveState : IState
{
    private SpiderController spiderController;
    public Spider_MoveState(SpiderController spiderController)
    {
        this.spiderController = spiderController;
    }
    public void Enter()
    {
        this.spiderController.ani.SetBool("move", true);
    }

    public void Execute()
    {
        Move();
    }
    public void Exit()
    {
        this.spiderController.ani.SetBool("move", false);
    }
    void Move()
    {
        if (!this.spiderController.DetecPlayer()) return;
        this.spiderController.moveDirection = (this.spiderController.targetObject.transform.position - this.spiderController.transform.position).normalized;
        this.spiderController.rb.velocity = this.spiderController.moveDirection * this.spiderController.speedMove;
    }
}
