using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehavior : BatController
{
    protected override void Start()
    {
        base.Start();
       // ChangeState(new Bat_IdleState(this));
    }
}
