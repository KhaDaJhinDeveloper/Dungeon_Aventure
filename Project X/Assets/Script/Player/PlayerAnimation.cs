
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator ani;
    private string moveAni = "isMoving";
    private string moveXAni = "MoveX";
    private string moveYAni = "MoveY";
    private bool isMoving;
    private Tween fadeTween;
    [SerializeField] private AudioSource audioSource;
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
            if(!this.isMoving)
            {
                this.isMoving = true;
                this.fadeTween?.Kill();
                if (!this.audioSource.isPlaying)
                    this.audioSource.Play();
                this.fadeTween = this.audioSource.DOFade(SoundManager.Instance.volumeSFX, 0.05f);
            }
        }
        else 
        {
            if (this.isMoving)
            {
                this.isMoving = false;
                this.fadeTween?.Kill();
                if (this.audioSource.isPlaying)
                {
                    this.fadeTween = this.audioSource.DOFade(0f, 0.25f)
                        .OnComplete(() => this.audioSource.Stop());
                }
            }
            this.ani.SetBool(this.moveAni, false); 
        }
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
