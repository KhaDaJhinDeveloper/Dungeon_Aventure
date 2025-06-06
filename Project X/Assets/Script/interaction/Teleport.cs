using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string targetObjectName; 
    private Transform player;
    private bool canTeleport = false;
    private Transform target;
    private bool istele = false;
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag(TagManager.TAG_PLAYER).transform;
        this.target = GameObject.Find(targetObjectName).transform;
    }
    void Update()
    {
        if (this.canTeleport == true)  
            TeleportPlayer();
    }
    private void TeleportPlayer()
    {       
        if (this.target != null)  
            StartCoroutine(Tele(1f));  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            // this.canTeleport = true;
            StartCoroutine(Tele(1f));
    }
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            this.canTeleport = false;
    }*/
    IEnumerator Tele(float duration)
    {
        if(!this.istele)
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_PlayerAnimationDrop);
            this.istele = true;
            yield return new WaitForSeconds(duration);
            this.player.position = this.target.transform.position;
            this.istele = false;
        }     
    }
}
