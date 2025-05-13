using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : EnemyController
{
    private GhostStats GhostStats;
    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState(this));
        this.GhostStats = GetComponent<GhostStats>();
    }
    protected override void Update()
    {
        base.Update();
        if(IsPlayerRangeChase(6f))
        {
            ChangeState(new FlyState(this));
        } 
        else
            ChangeState(new IdleState(this));
        if (this.GhostStats.IsDie)
            currentState = null;
    }
  
}
