using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BaseInteraction
{
    private bool canOpen;
    private bool canDrop;
    private Animator ani;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (this.canOpen)
            {
                if (this.canDrop)
                    DropItem();
                else Debug.Log("not item");
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
        this.canDrop = false;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.ani = GetComponent<Animator>();
        this.canDrop = true;
    }
}
