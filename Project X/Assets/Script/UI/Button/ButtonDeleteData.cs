using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteData : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        Debug.Log("Delete Data"); 
        PlayerPrefs.DeleteAll();
    }
}
