using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehavior : EnemyController
{
    private RatStats ratStats;
    private bool facingRight;
    protected override void Start()
    {
        base.Start();       
        this.ratStats = GetComponent<RatStats>();       
    }
    protected override void Update()
    {
        base.Update();
        if (this.ratStats.IsDie)
            currentState = null;
        ChangeState(new PatrolState(this));
        Flip();
    }
    public override void Flip()
    {
        if (rb.velocityX > 0 && facingRight)
        {
            sr.flipX = false;
            facingRight = !facingRight;
        }
        else if (rb.velocityX < 0 && !facingRight)
        {
            sr.flipX = true;
            facingRight = !facingRight;
        }
    }
}
