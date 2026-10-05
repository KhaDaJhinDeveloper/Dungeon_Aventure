
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator ani;
    private string moveAni = "isMoving";
    private string moveXAni = "MoveX";
    private string moveYAni = "MoveY";
    private Vector2 lastMoveDirection = Vector2.down;
    void Start()
    {
        this.ani = GetComponentInChildren<Animator>();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_PlayerAnimationDrop, AnimationDrop);
    }
    public void UpdateAnimation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            this.lastMoveDirection = direction.normalized;
            this.ani.SetBool(this.moveAni, true);
        }    
        else this.ani.SetBool(this.moveAni, false);
        this.ani.SetFloat(this.moveXAni, lastMoveDirection.x);
        this.ani.SetFloat(this.moveYAni, lastMoveDirection.y);
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
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_PlayerAnimationDrop, AnimationDrop);
    }
}
