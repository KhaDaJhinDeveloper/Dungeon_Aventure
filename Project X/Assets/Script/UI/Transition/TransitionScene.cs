using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScene : Singleton<TransitionScene>
{
    private const string OpenParam = "open";
    private const string CloseParam = "close";
    private const string OpenState = "Close";
    private const string CloseState = "Open";

    [SerializeField] private GameObject transitionUI;
    private Animator ani;

    private bool isLoad;

    protected override void Awake()
    {
        base.Awake();
        this.ani = this.transitionUI.GetComponent<Animator>();
    }

    private void Start()
    {
        this.transitionUI.SetActive(false);
    }
    public void PlayTransition(Action onAction)
    {
        TimeManager.TimeResume();
        StartCoroutine(TransitionEffect(onAction));
    }
    public IEnumerator TransitionEffect(Action onAction)
    {
        if (this.isLoad)
            yield break;

        this.isLoad = true;

        try
        {
            //SoundManager.Instance.StopPlayMusicBG();
            //SoundManager.Instance.PlayMusicSFX(SoundManager.Instance.sfx_LoadLevel);
            this.gameObject.SetActive(true);
            this.transitionUI.SetActive(true);

            this.ani.SetBool(OpenParam, false);
            this.ani.SetBool(CloseParam, false);
            yield return null;

            this.ani.SetBool(OpenParam, true);
            yield return WaitForStateComplete(OpenState);

            onAction?.Invoke();

            this.ani.SetBool(OpenParam, false);
            this.ani.SetBool(CloseParam, true);
            yield return WaitForStateComplete(CloseState);

            this.ani.SetBool(CloseParam, false);
            this.transitionUI.SetActive(false);
        }
        finally
        {
            this.isLoad = false;
        }
    }

    private IEnumerator WaitForStateComplete(string stateName)
    {
        yield return new WaitUntil(() =>
            ani.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        yield return new WaitUntil(() =>
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            && !ani.IsInTransition(0));
    }
}
