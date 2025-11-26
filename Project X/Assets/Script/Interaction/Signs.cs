using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signs : BaseInteraction
{
    [SerializeField] Content content;
    private bool isTrigger;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (isTrigger)
            if (Input.GetKeyDown(KeyCode.E))
                EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_LoadIndex, this.content.spriteDescription, this.content.textDescription);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.isTrigger = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
            this.isTrigger = false;
        }
    }
}
