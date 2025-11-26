using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehavior : SpiderController
{
    [SerializeField] private GameObject HPbar;
    protected override void Start()
    {
        base.Start();
        ChangeState(new Spider_HideState(this));
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
    protected override void Update()
    {
        base.Update();
        HideHPbar();
        if (CanAttack()) this.canAttack = true;
        if (this.DetecPlayer())
        {
            FlipPhysics();
            ChangeState(new Spider_MoveState(this));
            if (this.canAttack)
            {
                if (!this.attackComplated)
                {
                    StartCoroutine(Attack());
                }
            }
        }
        else ChangeState(new Spider_HideState(this));
        if(this.attackComplated)
            StartCoroutine(Rest());
    }
    IEnumerator Rest()
    {
        ChangeState(new Spider_IdleState(this));
        yield return new WaitForSeconds(2f);
        this.attackComplated = false;
    }
    IEnumerator Attack()
    {
        ChangeState(new Spider_AttackState(this));
        yield return new WaitForSeconds(1.5f);
        this.attackComplated = true;
        this.canAttack = false;
    }
    void HideHPbar()
    {
        if(this.DetecPlayer()) 
            this.HPbar.SetActive(true);
        else this.HPbar.SetActive(false);
    }
}
