using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBow : BaseTrap,IShoot
{
    [SerializeField] private float speedBullet;
    [SerializeField] private float timeAttack;
    private float currentTime;
    private bool canAttack;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.currentTime = 0f;
    }   
    protected override void Update()
    {       
        if (this.canAttack)
        {
            this.currentTime += Time.deltaTime;
            if (this.currentTime > this.timeAttack)
            {
                this.ani.SetTrigger("attack");
                this.currentTime = 0f;
            }    
        } 
            
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.canAttack = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.canAttack = false;
            this.currentTime = 0f;
        }
    }
    public void Shooting()
    {
        GameObject bulleBow = ObjectPooling.ObjectPooling_Instance.GetPool("BowBullet");
        bulleBow.transform.position = this.transform.position;
        Rigidbody2D rbbullet = bulleBow.GetComponent<Rigidbody2D>();
        rbbullet.velocity = transform.right * this.speedBullet;
    }
}


