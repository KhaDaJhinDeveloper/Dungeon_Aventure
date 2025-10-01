using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : BaseInteraction
{
    [SerializeField] private string targetObjectName; 
    private Transform player;
    private Transform target;
    private bool istele = false;
    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.player = GameObject.FindGameObjectWithTag(TagManager.TAG_PLAYER).transform;
        this.target = GameObject.Find(targetObjectName).transform;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            StartCoroutine(Tele(1f));
    }
    IEnumerator Tele(float duration)
    {
        if(!this.istele)
        {
            if (this.target != null)
            {
                EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_PlayerAnimationDrop);
                this.istele = true;
                yield return new WaitForSeconds(duration);
                this.player.position = this.target.transform.position;
                this.istele = false;
            }
            else Debug.Log("Not Tele" + this.targetObjectName);
        }     
    }
}
