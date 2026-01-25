
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private bool facingRight;
    void Start()
    {
        this.ani = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_PlayerAnimationDrop, AnimationDrop);
    }
    void Update()
    {
        AnimationMove();
    }
    void AnimationMove()    
    {
        bool moveleft = this.rb.velocity.x != 0;
        this.ani.SetBool("move",moveleft);
        if (this.rb.velocity.x > 0 && this.facingRight)
            Flip();
        else if (this.rb.velocity.x < 0 && !this.facingRight) Flip();
        bool runup = this.rb.velocity.y > 0 && rb.velocity.x == 0;
        this.ani.SetBool ("runup",runup);
        bool rundown = this.rb.velocity.y < 0 && rb.velocity.x == 0;
        this.ani.SetBool("rundown", rundown);
    }
    public void AnimationDrop()
    {
        this.ani.SetTrigger("drop");
    }
    public void AnimationTakeHit()
    {
        this.ani.SetTrigger("takehit");
    }
    public void AnimationDeath()
    {
        this.ani.SetTrigger("death");
    }    
    void Flip()
    {
        this.facingRight = !this.facingRight;
        this.transform.Rotate(0, 180, 0);
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_PlayerAnimationDrop, AnimationDrop);
    }
}
