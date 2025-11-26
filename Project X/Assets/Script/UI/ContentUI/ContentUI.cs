using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContentUI : MonoBehaviour
{
    [SerializeField] Image imageDescription;
    [SerializeField] TextMeshProUGUI textDescription;
    [SerializeField] GameObject contentBoard;
    [SerializeField] Transform startPos;
    private void Start()
    {
        SubEvent();
    }
    public void LoadIndex(Sprite sprite, string text)
    {
        this.imageDescription.sprite = sprite;
        this.textDescription.text = text;
        ShowContentBoard();
    }
    public void ShowContentBoard()
    {
        this.contentBoard.transform.DOKill();
        this.contentBoard.transform.position = this.startPos.transform.position;
        this.contentBoard.SetActive(true);
        this.contentBoard.transform.DOLocalMove(new Vector3(0, 30, 0), 0.5f).SetUpdate(true);
        TimeManager.TimePause();
    }   
    public void HideContentBroad()
    {
        this.contentBoard.transform.DOKill();
        this.contentBoard.transform.DOMove(this.startPos.transform.position, 0.5f).SetUpdate(true).OnComplete(() => { this.contentBoard.SetActive(false);
                                                                                                                           TimeManager.TimeResume();});                                                                                                                                                                                                                                
    }    
    void SubEvent()
    {
        EventManager.OP_EventManager.Subscribe<Sprite, string>(NameEvent.Event_LoadIndex, LoadIndex);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_ShowContentBoard, ShowContentBoard);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_HideContentBroad, HideContentBroad);
    }    
    void UnsubEvent()
    {
        EventManager.OP_EventManager.Unsubscribe<Sprite, string>(NameEvent.Event_LoadIndex, LoadIndex);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_ShowContentBoard, ShowContentBoard);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_HideContentBroad, HideContentBroad);
    }    
    private void OnDestroy()
    {
        UnsubEvent();
    }
}
