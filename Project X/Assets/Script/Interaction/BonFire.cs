using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFire : BaseInteraction
{
    private Collider2D colli;
    private bool canActive;
    private CountdownTimer timer;
    private Animator ani;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.timer = GameObject.FindFirstObjectByType<CountdownTimer>();
        this.ani = GetComponent<Animator>();
    }
    protected override void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(this.canActive)
            {
                this.canInteract = false;
                this.ani.SetBool("fire", true);
                DisableObject();
            }
        }
        if(this.canActive && !this.canInteract)
            this.timer.IncreaseTime(0.1f);     
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.canActive = true;
        }    
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
            this.canActive= false;
        }
    }
    public override void DisableObject()
    {
        base.DisableObject();
        if (colli == null) this.colli = GetComponent<Collider2D>();
        colli.enabled = false;
    }
    public override void OnLoadData(bool canInteract)
    {
        base.OnLoadData(canInteract);
        if(!canInteract)
        {
            this.canInteract = false;
            this.ani?.SetBool("fire", true);
            DisableObject();
        }    
    }
}
