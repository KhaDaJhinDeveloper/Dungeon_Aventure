using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : BaseInteraction
{
    [SerializeField] private NamePointTele targetObjectName;
    private Transform target;
    private bool istele = false;
    protected override void Start()
    {
        base.Start();
        this.target = GameObject.Find(targetObjectName.ToString())?.transform;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
             StartCoroutine(Tele(2f, collision.gameObject));
        }
    }
    IEnumerator Tele(float duration, GameObject player)
    {
        if(!this.istele)
        {
            if (this.target != null)
            {
                EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_PlayerAnimationDrop);
                this.istele = true;
                yield return new WaitForSeconds(.4f);
                TransitionScene.Instance.PlayTransition(() =>
                {
                    player.transform.position = this.target.transform.position;
                    this.istele = false;
                });
            }
        }     
    }
}
