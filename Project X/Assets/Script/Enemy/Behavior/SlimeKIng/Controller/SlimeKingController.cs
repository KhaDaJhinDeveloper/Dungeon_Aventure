using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingController : MonoBehaviour
{
    #region State
    private IState currentState;
    #endregion
    #region Components
    private Collider2D coli;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator ani;
    #endregion
    #region Move
    private Vector2 moveDirection;
    #endregion
    #region Target
    private GameObject player;
    #endregion
    protected virtual void Start()
    {
        LoadComponents();
        SetUp();
    }
    protected virtual void Update()
    {
        if (this.currentState != null)
            this.currentState.Execute();
    }
    protected virtual void ChangeState(IState state)
    {
        if (currentState != null && currentState.GetType() == state.GetType())
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
    protected virtual void LoadComponents()
    {
        this.coli = GetComponent<Collider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.ani = GetComponentInChildren<Animator>();
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        this.player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
    }
    protected virtual void SetUp()
    {

    }
    public virtual void LockVelocity() => this.rb.velocity = Vector3.zero;
    public virtual void UnLockVelocity() => this.rb.velocity = this.moveDirection;
}
