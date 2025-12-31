using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonContinue : BaseButton
{
    protected override void Start()
    {
        base.Start();
        this.button.interactable = GameSaveManager.Instance.HasData();
    }
    protected override void OnClick()
    {
        DebugLogger.Log("continue");
        string nameScene = GameSceneStateManager.S_GameSceneStateManager.GetSceneNameData();
        GameSaveManager.Instance.RequestLoadOnNextScene();
        SceneManager.LoadScene(nameScene);
    }
}
