using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_FakeDieState : IState
{
    private SkeletonController skeletonController;
    public Skeleton_FakeDieState(SkeletonController skeletonController)
    {
        this.skeletonController = skeletonController;
    }
    public void Enter()
    {
        this.skeletonController.AllowKnockBack = false;
        this.skeletonController.LockVelocity();
        this.skeletonController.Colli.enabled = false;
        this.skeletonController.Ani.SetBool("fakedie", true);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        this.skeletonController.Colli.enabled = true;
        this.skeletonController.Ani.SetBool("fakedie", false);
    }
}
