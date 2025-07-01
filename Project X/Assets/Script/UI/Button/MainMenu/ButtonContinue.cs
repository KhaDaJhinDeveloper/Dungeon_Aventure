using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContinue : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        Debug.Log("continue");
    }
}
