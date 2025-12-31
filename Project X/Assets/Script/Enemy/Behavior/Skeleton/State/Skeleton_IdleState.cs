using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_IdleState : IState
{
    private SkeletonController skeletonController;
    public Skeleton_IdleState(SkeletonController skeletonController)
    {
        this.skeletonController = skeletonController;
    }
    public void Enter()
    {
        this.skeletonController.AllowKnockBack = false;
        this.skeletonController.LockVelocity();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
