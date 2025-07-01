using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : EnemyController
{
    private GhostStats ghostStats;
    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState(this));
        this.ghostStats = GetComponent<GhostStats>();
    }
    protected override void Update()
    {
        base.Update();
        if (IsPlayerRangeChase(detectionRange))
        {
            ChangeState(new FlyState(this));
        }
        else
            ChangeState(new IdleState(this));
        if (this.ghostStats.IsDie)
            currentState = null;
        Flip();
    }
    public override void Flip()
    {
        if (transform.position.x > playerTransform.position.x)
            sr.flipX = true;
        else if (transform.position.x < playerTransform.position.x)
            sr.flipX = false;
    }
}
