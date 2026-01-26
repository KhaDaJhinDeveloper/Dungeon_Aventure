using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SceneBossState : MonoBehaviour
{
    private bool bossIsDie;
    [SerializeField] GameObject bossUI;
    [SerializeField] GameObject doorblock;
    private void Start()
    {
        EndBossFight();
    }
    private void Update()
    {
        if (this.bossIsDie)
            EndBossFight();
    }
    public bool ComplateBossFight() => this.bossIsDie = true;
    public void StartBossFight()
    {
        this.doorblock.SetActive(true);
        ShowUI();
    }
    public void EndBossFight()
    {
        this.doorblock.SetActive(false);
        HideUI();
    }
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
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_StartBossFight, StartBossFight);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_EndBossFight, EndBossFight);
    }
    private void OnDisable()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_StartBossFight, StartBossFight);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_EndBossFight, EndBossFight);
    }
}
