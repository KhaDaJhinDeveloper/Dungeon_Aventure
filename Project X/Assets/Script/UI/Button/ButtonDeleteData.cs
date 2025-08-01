using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteData : BaseButton
{
    // Start is called before the first frame update
    protected override void OnClick()
    {
        PlayerPrefs.DeleteAll();
    }
}
