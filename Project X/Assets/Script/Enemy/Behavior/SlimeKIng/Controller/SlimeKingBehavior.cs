using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingBehavior : SlimeKingController
{
    protected override void Start()
    {
        base.Start();
        //ChangeState(new SlimeKing_IdleState(this));
        //ChangeState(new SlimeKing_MoveState(this));
        ChangeState(new SlimeKing_SpawnState(this));
        //ChangeState(new SlimeKing_AttackState(this));
    }
}
