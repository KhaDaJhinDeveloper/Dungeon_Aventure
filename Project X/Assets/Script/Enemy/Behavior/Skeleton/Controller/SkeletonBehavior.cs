using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : SkeletonController
{
    protected override void Start()
    {
        base.Start();
        ChangeState(new Skeleton_IdleState(this));
    }
    protected override void Update()
    {
        base.Update();
        if (this.stats.Life <= 0)
            StartCoroutine(FakeDie());
        else if (this.isTrigger)
        {
            ChangeState(new Skeleton_ChaseState(this));
        }
        else
            ChangeState(new Skeleton_IdleState(this));
        FlipPhysics();
    }
    IEnumerator FakeDie()
    {
        ChangeState(new Skeleton_FakeDieState(this));
        yield return new WaitForSeconds(2f);
        FlipTransform();
        ChangeState(new Skeleton_RageState(this));
    }
}
