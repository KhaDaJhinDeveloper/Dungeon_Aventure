using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected IState currentState;
    public Transform playerTransform;
    public Rigidbody2D rb;
    protected Animator ani;

    protected SpriteRenderer sr;
    protected virtual void Start()
    {
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        playerTransform = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform;
    }
    protected virtual void Update()
    {
        if (currentState != null)
            currentState.Execute();
    }
    protected virtual void ChangeState(IState state)
    {
        if(currentState != null && currentState.GetType() == state.GetType())
            return;
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    protected virtual bool IsPlayerRangeChase(float range)
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < range)
            return true;
        else
            return false;
    }
    protected virtual bool IsPlayerRangeAttack(float range)
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < range)
            return true;
        else
            return false;
    }
    protected virtual Vector2 ChooseRanDomDirection(Vector2 Vectorrandom)
    {
        Vectorrandom = Random.insideUnitCircle.normalized;
        return Vectorrandom;
    }     
    public virtual void Flip()
    {
        if(transform.position.x > playerTransform.position.x)
            sr.flipX = true;
        else if(transform.position.x < playerTransform.position.x)
            sr.flipX=false;
    }
}
    