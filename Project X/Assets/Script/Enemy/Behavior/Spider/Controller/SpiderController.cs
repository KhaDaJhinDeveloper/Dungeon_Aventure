using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpiderController : MonoBehaviour
{
    protected IState currentState;
    [Header("Components")]
    public GameObject sprite;
    public Animator ani;
    public Rigidbody2D rb;
    public BaseStats stats;
    public GameObject targetObject;
    public CircleCollider2D colli;
    [Header("Movement")]
    public int speedMove;
    public Vector2 moveDirection;
    [Header("Detec")]
    public Transform detecTransform;
    public float radius;
    public LayerMask layerPlayer;
    public bool isDetecPlayer;
    public bool showDebugRay;
    [Header("Attack")]
    public bool attackComplated;
    public bool canAttack;
    [Header("Flip")]
    public bool facingRight;
    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void Update()
    {
        if (this.currentState != null)
            this.currentState.Execute();
    }
    protected virtual void LoadComponent()
    {
        this.stats = GetComponent<BaseStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.colli = GetComponent<CircleCollider2D>();
        this.targetObject = GameObject.FindWithTag(TagManager.TAG_PLAYER);
        this.speedMove = this.stats.Speed;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
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
    public virtual bool CanAttack()
    {
        float distance = Vector2.Distance(this.transform.position, this.targetObject.transform.position);
        return distance <1.2f;
    }   
    public virtual void StopAttack()
    {
        this.attackComplated = false;
    }
    public virtual void LockVelocity() => this.rb.velocity = Vector2.zero;
    public virtual void UnlockVelocity() => this.rb.velocity = this.moveDirection * this.speedMove;
    public virtual bool DetecPlayer()
    {
        this.isDetecPlayer = Physics2D.OverlapCircle(this.detecTransform.position, this.radius, this.layerPlayer);
        return this.isDetecPlayer;
    }
    private void OnDrawGizmos()
    {
        if(!this.showDebugRay) return; 
        Gizmos.color = DetecPlayer()? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(this.detecTransform.position, this.radius);
    }
    protected virtual void FlipPhysics()
    {
        if (this.rb.velocity.x > 0 && facingRight)
        {
            this.sprite.transform.Rotate(0,180,0);
            this.facingRight = !facingRight;
        }
        else if (this.rb.velocity.x < 0 && !facingRight)
        {
            this.sprite.transform.Rotate(0, 180, 0);
            this.facingRight = !facingRight;
        }
    }
}
