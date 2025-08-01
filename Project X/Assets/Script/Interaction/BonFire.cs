using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFire : BaseInteraction
{
    private bool activeBorn;
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
                this.activeBorn = true;
                this.ani.SetBool("fire", true);
            }
        }
        if(this.canActive && this.activeBorn)
            this.timer.IncreaseTime(0.1f);     
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            if(!this.activeBorn)
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
}
