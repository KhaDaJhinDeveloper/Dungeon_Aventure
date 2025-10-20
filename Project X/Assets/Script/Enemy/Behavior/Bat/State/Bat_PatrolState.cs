using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_PatrolState : IState
{
    protected BatController batController;
    private float currentTime = 0;
    public Bat_PatrolState(BatController batController) 
    { 
        this.batController = batController;
    }
    public void Enter()
    {
        Debug.Log("Start Patrol");
        float randomAngle = Random.Range(0f, 360f);
        this.batController.MoveDirection = new Vector3 (Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0f).normalized;
    }
    public void Execute()
    {
        Move();
        CheckLineOfSight();
    }

    public void Exit()
    {
        Debug.Log("End Patrol");
    }
    void Move()
    {
        this.currentTime += Time.deltaTime;
        if (this.currentTime >= this.batController.TimeChangeDirection)
            ChangeMoveDirection();
        else if (IsWall())
            ChooseRandomDirection();
        this.batController.Rb.velocity = this.batController.MoveDirection.normalized * this.batController.SpeedMove;
    }
    void ChangeMoveDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        this.batController.MoveDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0f).normalized;
        this.currentTime = 0f;
    }
    void ChooseRandomDirection()
    {
        this.batController.MoveDirection = -this.batController.MoveDirection;
    }
    bool IsWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.batController.transform.position, this.batController.MoveDirection, this.batController.RayDistance, this.batController.NameLayer);
        return hit.collider != null;
    }
    void CheckLineOfSight()
    {
        this.batController.IsDetecPlayer = false;
        if (this.batController.TargetObject == null) return;
        if (this.batController.IsTrigger)
        {
            float distanceTarget = Vector3.Distance(this.batController.TargetObject.transform.position, this.batController.transform.position);
            Vector3 directionTarget = (this.batController.TargetObject.transform.position- this.batController.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(this.batController.transform.position, directionTarget, distanceTarget, this.batController.LayerWall);
            if(hit.collider == null)
                this.batController.IsDetecPlayer = true;
        }    
    }
}
