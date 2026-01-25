using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing_MoveState : IState
{
    private SlimeKingController controller;
    public SlimeKing_MoveState(SlimeKingController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        Move();
    }

    public void Exit()
    {
        
    }
    void Move()
    {
        Vector3 pos = this.controller.transform.position;
        Vector3 target = this.controller.Player.transform.position;
        Vector3 direction = ( target - pos).normalized;
        this.controller.Rb.velocity = direction * this.controller.Speed;
    }
}
