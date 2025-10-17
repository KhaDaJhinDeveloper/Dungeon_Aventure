using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehavior : BatController
{
    protected override void Start()
    {
        base.Start();
        ChangeState(new Bat_PatrolState(this));
    }
    protected override void Update()
    {
        base.Update();
        if (this.isDetecPlayer)
        {
            ChangeState(new Bat_AttackState(this));
        }

        if (this.attackComplated)
        {
            StartCoroutine(Rest());
        }
    }
    IEnumerator Rest()
    {
        ChangeState(new Bat_IdleState(this));
        yield return new WaitForSeconds(3f);
        ChangeState(new Bat_PatrolState(this));
        this.attackComplated = false;
    }
}
