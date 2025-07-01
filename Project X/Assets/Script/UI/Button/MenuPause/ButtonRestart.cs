using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestart : BaseButton
{
    protected override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TimeManager.TimeResume();
    }
}
