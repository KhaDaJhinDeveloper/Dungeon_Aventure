using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonReturnMainMenu : BaseButton
{
    protected override void OnClick()
    {
        TimeManager.TimeResume();
        SceneManager.LoadScene("MainMenu");
    }
}
