using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SceneBossState : MonoBehaviour
{
    [SerializeField] GameObject bossUI;
    [SerializeField] GameObject doorblock;
    public bool isEnd;
    private void Start()
    {
        EndBossFight();
    }
    #region BossFightState
    public void StartBossFight()
    {
        this.doorblock.SetActive(true);
        ShowUI();
        this.isEnd = false;
    }
    public void EndBossFight()
    {
        this.doorblock.SetActive(false);
        HideUI();
        this.isEnd = true;
    }
    public void LoadState(bool isEnd)
    {
        if (isEnd)
            EndBossFight();
        else
            StartBossFight();
        this.isEnd = isEnd;
    }
    #endregion
    #region Event
    void HideUI()
    {
        this.bossUI.transform.DOKill();
        this.bossUI.transform.DOLocalMove(new Vector3(0, -60, 0), 0.5f);
    }
    void ShowUI()
    {
        this.bossUI.transform.DOKill();
        this.bossUI.transform.DOLocalMove(new Vector3(0, 140, 0), 0.5f);
    }
    private void OnEnable()
    {
        EventManager.Instance.Subscribe(NameEvent.Event_StartBossFight, StartBossFight);
        EventManager.Instance.Subscribe(NameEvent.Event_EndBossFight, EndBossFight);
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe(NameEvent.Event_StartBossFight, StartBossFight);
        EventManager.Instance?.Unsubscribe(NameEvent.Event_EndBossFight, EndBossFight);
    }
    #endregion
}
