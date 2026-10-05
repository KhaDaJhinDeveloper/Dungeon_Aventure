using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : BaseInteraction
{
    private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private int manaQty;
    [SerializeField] private KeyPool keyPool;
    protected override void Update()
    {
        base.Update();
        if(this.canInteract && this.target != null)
            this.transform.position = Vector2.MoveTowards(this.transform.position, this.target.position, this.speed * Time.deltaTime);
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            stats?.ManaRecovery(2);
            ObjectPooling.Instance?.ReturnToPool(this.keyPool,this.gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.target = collision.gameObject.GetComponent<Transform>();
            this.canInteract = true;
        }
    }
    private void OnDisable()
    {
        this.canInteract = false;
        this.target = null;
    }
}
