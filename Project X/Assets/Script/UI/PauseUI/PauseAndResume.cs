using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseAndResume : MonoBehaviour
{
    private bool gamepause;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject background;
    private Vector3 startPos;
    private void Start()
    {
        //this.pauseUI = GameObject.FindGameObjectWithTag(TagManager.TAG_MENUUI);
        //this.pauseUI.SetActive(false);
        this.startPos = this.pauseUI.transform.position;
    }
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
        this.pauseUI.transform.DOKill();
        this.gamepause = true;
        this.pauseUI.transform.DOMove(this.startPos, 0.5f).SetUpdate(true).OnComplete(() => { this.background.SetActive(false); this.pauseUI.SetActive(false); TimeManager.TimeResume(); }); 
    }
    void Pause()
    {
        this.pauseUI.transform.DOKill();
        this.gamepause = false;
        this.background.SetActive(true);
        this.pauseUI.SetActive(true);
        this.pauseUI.transform.DOLocalMove(new Vector3(0, 30, 0), 0.5f).SetUpdate(true);
        TimeManager.TimePause();
    }
    private void OnDestroy()
    {
        this.pauseUI.transform.DOKill();
    }
}
