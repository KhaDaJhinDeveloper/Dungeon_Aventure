using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNewGame : BaseButton
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        Debug.Log("newgame");
    }
}
