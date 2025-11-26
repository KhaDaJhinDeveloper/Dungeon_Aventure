using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChildBehavior : SlimeController
{
    float timeChange;
    protected override void Start()
    {
        base.Start();
        this.timeChange = Random.Range(0.5f, 3f);
        ChangeState(new Slime_IdleState(this));
    }
    protected override void Update()
    {
        base.Update();
        this.currentTime += Time.deltaTime;
        if (this.currentTime >= this.maxTime)
            StartCoroutine(Rest());
    }
    IEnumerator Rest()
    {
        ChangeState(new Slime_IdleState(this));
        yield return new WaitForSeconds(this.timeChange);
        this.currentTime = 0;
        this.timeChange = Random.Range(0.5f, 3f);
        ChangeState(new Slime_MoveState(this));
    }
}
