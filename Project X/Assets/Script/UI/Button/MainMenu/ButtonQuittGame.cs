using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuittGame : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        DebugLogger.Log("quit");
    }
}
