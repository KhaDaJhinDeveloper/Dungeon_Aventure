using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_ChaseState : IState
{

    private SkeletonController skeletonController;
    public Skeleton_ChaseState(SkeletonController skeletonController)
    {
        this.skeletonController = skeletonController;
    }
    public void Enter()
    {
        this.skeletonController.AllowKnockBack = true;
        this.skeletonController.UnlockVelocity();
        this.skeletonController.Ani.SetBool("move", true);
    }

    public void Execute()
    {
        //bool check = this.skeletonController.IsTrigger && IsDetecPlayer() && this.skeletonController.Stats.Life >0;
        if(IsDetecPlayer())
        {
            Chase();
        }
        else
        {
            this.skeletonController.LockVelocity();
        }
        //this.skeletonController.Ani.SetBool("move", check);
    }

    public void Exit()
    {
        this.skeletonController.Ani.SetBool("move", false);
    }
    void Chase()
    {
        Vector2 pos = this.skeletonController.transform.position;
        Vector2 target = this.skeletonController.Target.position;
        Vector2 moveDirection = (target - pos).normalized;
        this.skeletonController.Rb.velocity = moveDirection * this.skeletonController.Stats.Speed;
    }
    bool IsDetecPlayer()
    {
        if(!this.skeletonController.IsTrigger) return false;
        Vector2 pos = this.skeletonController.transform.position;
        Vector2 target = this.skeletonController.Target.position;
        Vector2 direction = (target - pos).normalized;
        float distance = Vector2.Distance(target, pos);
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, this.skeletonController.WallLayer);
        return hit.collider == null;
    }
}
