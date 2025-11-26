using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : SlimeController
{
    private float timeChange;
    private BaseStats stats;
    [SerializeField] private int quantity;
    protected override void Start()
    {
        base.Start();
        this.timeChange = Random.Range(0.5f, 3f);
        ChangeState(new Slime_MoveState(this));
    }
    protected override void Update()
    {
        base.Update();
        this.currentTime += Time.deltaTime;
        if (this.currentTime >= this.maxTime)
            StartCoroutine(Rest());
        if(this.slimeStats.IsDie)
            StartCoroutine(Spawn());
    }
    IEnumerator Rest()
    {
        ChangeState(new Slime_IdleState(this));
        yield return new WaitForSeconds(this.timeChange);
        this.currentTime = 0;
        this.timeChange = Random.Range(0.5f, 3f);
        ChangeState(new Slime_MoveState(this));
    }
    IEnumerator Spawn()
    {
        this.SlimeStats.IsDie = false;
        ChangeState(new Slime_IdleState(this));
        yield return new WaitForSeconds(0.8f);
        ChangeState(new Slime_SpawnState(this, this.quantity));
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
}
