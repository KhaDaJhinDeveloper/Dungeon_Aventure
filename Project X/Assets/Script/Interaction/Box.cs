using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BaseInteraction
{
    private bool canOpen;
    private Animator ani;
    private Collider2D colli;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        if (!this.canInteract) return;
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (this.canOpen && this.canInteract)
            {
                DropItem();
                DisableObject();
            }                
        }    
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.canOpen = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
            this.canOpen = false;
        }
    }
    void DropItem()
    {
        this.ani.SetBool("open", true);
        ItemDropSpawn.itemDropSpawn_Instance.DropWeapon(this.transform.position);
        this.canInteract = false;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.ani = GetComponent<Animator>();
        this.colli = GetComponent<Collider2D>();
        this.canInteract = true;
    }
    public override void DisableObject()
    {
        base.DisableObject();
        this.colli.enabled = false;
    }
}
