using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bat_AttackState : IState
{
    protected BatController batController;
    private Vector3 retreatTargetPosition;
    private Vector3 keyPosition;
    private bool hasKeyPos;
    private bool isRetreating;
    public Bat_AttackState(BatController batController)
    {
        this.batController = batController;
    }
    public void Enter()
    {
        this.hasKeyPos = false;
    }

    public void Execute()
    {
        this.batController.Rb.velocity = Vector2.zero;
        MarkTheLocation();
    }

    public void Exit()
    {
        this.batController.Ani.SetBool("attack", false);
    }
    void MarkTheLocation()
    {
        if(this.batController.TargetObject != null && this.batController.IsDetecPlayer && !this.hasKeyPos)
        {
            this.keyPosition = this.batController.TargetObject.transform.position;
            Vector3 directionToPlayer = (this.batController.transform.position - this.batController.TargetObject.transform.position).normalized;
            this.retreatTargetPosition = this.batController.transform.position + directionToPlayer * 1f;
            this.hasKeyPos = true;
            this.isRetreating = true;
        }
        Attack();
    }
    void Attack()
    {
        if(this.isRetreating)
        {
            this.batController.Ani.SetBool("attack", true);
            this.batController.transform.position = Vector3.MoveTowards(this.batController.transform.position, this.retreatTargetPosition, 1 * Time.deltaTime);
            if (Vector3.Distance(this.batController.transform.position, this.retreatTargetPosition) <= 0.1f)
            {
                this.isRetreating = false;
            }
        }   
        else
        {
            this.batController.transform.position = Vector3.MoveTowards(this.batController.transform.position, this.keyPosition, 4 * Time.deltaTime);
            if (Vector3.Distance(this.batController.transform.position, this.keyPosition) <= 0.5f)
                this.batController.AttackComplated = true;
        }    
    }
}
