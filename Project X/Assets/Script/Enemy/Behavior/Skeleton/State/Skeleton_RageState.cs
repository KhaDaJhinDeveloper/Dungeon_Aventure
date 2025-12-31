using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_RageState : IState
{
    private int speed;
    private Vector3 posPlayer;
    private SkeletonController skeletonController;
    public Skeleton_RageState(SkeletonController skeletonController)
    {
        this.skeletonController = skeletonController;
    }
    public void Enter()
    {
        this.skeletonController.Ani.SetBool("rage",true);
        this.skeletonController.UnlockVelocity();
        this.skeletonController.AllowKnockBack = true;
    }

    public void Execute()
    {
        Rage();
    }

    public void Exit()
    {
        this.skeletonController.Ani.SetBool("rage", false);
    }
    void Rage()
    {
        posPlayer = this.skeletonController.Target.position;
        this.skeletonController.Agent.SetDestination(posPlayer);
    }
}
