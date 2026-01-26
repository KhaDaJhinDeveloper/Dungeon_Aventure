using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingBehavior : SlimeKingController
{
    private bool isCoolDown = true;
    protected override void Start()
    {
        base.Start();
        this.currentCoolDownAttack = this.CoolDownAttack;
        this.currentCoolDownSpawn = this.CoolDownSpawn;
        ChangeState(new SlimeKing_IdleState(this));
    }
    protected override void Update()
    {
        base.Update();
        CheckCoolDown();
        FlipTransform();
        if (IsRangeChase())
        {
            if (this.allowSpawn)
            {
                StartCoroutine(Spawn());
            }
            else
                ChangeState(new SlimeKing_MoveState(this));
        }
        if (IsRangeAttack())
        {
            if (this.allowAttack)
            {
                if (!this.attackCompleted) 
                    StartCoroutine(Attack());
            }
        }
        if (this.stats.ThisIsDie())
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_EndBossFight);
    }
    void CheckCoolDown()
    {
        if (!this.isCoolDown) return;
        this.currentCoolDownSpawn -= Time.deltaTime;
        this.currentCoolDownAttack -= Time.deltaTime;
        if (this.currentCoolDownAttack < 0)
        {
            this.currentCoolDownAttack = 0;
            this.allowAttack = true;
        }
        if (this.currentCoolDownSpawn < 0)
        {
            this.currentCoolDownSpawn = 0;
            this.allowSpawn = true;
        }
    }
    IEnumerator Attack()
    {
        this.isCoolDown = false;
        this.attackCompleted = true;
        ChangeState(new SlimeKing_AttackState(this));
        yield return new WaitForSeconds(7f);
        ChangeState(new SlimeKing_IdleState (this));
        this.currentCoolDownAttack = this.CoolDownAttack;
        this.allowAttack = false;
        this.attackCompleted = false;
        this.isCoolDown = true;
    }
    IEnumerator Spawn()
    {
        this.isCoolDown = false;
        ChangeState(new SlimeKing_SpawnState(this));
        yield return new WaitForSeconds(3f);
        ChangeState(new SlimeKing_IdleState(this));
        this.currentCoolDownSpawn = this.CoolDownSpawn;
        this.allowSpawn = false;
        this.isCoolDown = true;
    }
}
