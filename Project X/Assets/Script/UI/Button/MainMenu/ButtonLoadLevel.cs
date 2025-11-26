using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadLevel : BaseButton
{
    [SerializeField] private NameScene nameScene;
    protected override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene(this.nameScene.ToString());
    }
}
