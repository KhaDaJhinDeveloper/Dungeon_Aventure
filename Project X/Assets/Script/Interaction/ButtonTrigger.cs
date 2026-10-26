using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : BaseInteraction
{
    private Transform posPlayer;
    protected override void Start()
    {
        base.Start();
        this.gameObject.SetActive(false);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.posPlayer = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform;
    }
    void Showbutton()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = this.posPlayer.position;
    }
    void HiddenButton()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.Instance.Subscribe(NameEvent.Event_ShowButtonTrigger, Showbutton);
        EventManager.Instance.Subscribe(NameEvent.Event_HiddenButtonTrigger, HiddenButton);
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe(NameEvent.Event_ShowButtonTrigger, Showbutton);
        EventManager.Instance?.Unsubscribe(NameEvent.Event_HiddenButtonTrigger, HiddenButton);
    }
}
