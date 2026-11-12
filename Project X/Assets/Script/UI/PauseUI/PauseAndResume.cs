using DG.Tweening;
using UnityEngine;

public class PauseAndResume : MonoBehaviour
{
    private bool isPause = true;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject background;
    [SerializeField] private Transform startPos;
    //private void Start()
    //{
    //    //this.pauseUI = GameObject.FindGameObjectWithTag(TagManager.TAG_MENUUI);
    //    //this.pauseUI.SetActive(false);
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.isPause)
                Pause();
            else Resume();      
        }
    }
    public void Resume()
    {
        this.isPause = true;
        SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Interface_MenuPause);
        this.pauseUI.transform.DOKill();
        this.pauseUI.transform.DOMove(this.startPos.transform.position, 0.5f).SetUpdate(true).OnComplete(() => 
        {
            this.background.SetActive(false); 
            this.pauseUI.SetActive(false);
            TimeManager.TimeResume(); 
        }); 
    }
    void Pause()
    {
        this.isPause = false;
        SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Interface_MenuPause);
        this.pauseUI.transform.DOKill();
        this.pauseUI.transform.position = this.startPos.transform.position;
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
