using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndResume : MonoBehaviour
{
    private bool gamepause;
    [SerializeField]private GameObject pauseUI;
    /*private void Start()
    {
        this.pauseUI = GameObject.FindGameObjectWithTag(TagManager.TAG_MENUUI);
        this.pauseUI.SetActive(false);
    }*/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.gamepause)
                Pause();
            else Resume();      
        }
    }
    void Resume()
    {
        this.gamepause = true;
        this.pauseUI.SetActive(false);
        TimeManager.TimeResume();
    }
    void Pause()
    {
        this.gamepause = false;
        this.pauseUI.SetActive(true);
        TimeManager.TimePause();
    }
}
