using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonNewGame : BaseButton
{

    DialogIntro intro;
    protected override void Start()
    {
        base.Start();
        this.intro = GameObject.FindFirstObjectByType<DialogIntro>();
    }
    protected override void OnClick()
    {
       this.intro.StartIntro();
    }
}
