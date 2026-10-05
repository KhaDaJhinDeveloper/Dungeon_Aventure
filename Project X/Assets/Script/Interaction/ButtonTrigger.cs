using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : BaseInteraction
{
    private Transform posPlayer;
    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.posPlayer = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform;
        this.gameObject.SetActive(false);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_ShowButtonTrigger, Showbutton);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_HiddenButtonTrigger, HiddenButton);
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
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_ShowButtonTrigger, Showbutton);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_HiddenButtonTrigger, HiddenButton);
    }
}
