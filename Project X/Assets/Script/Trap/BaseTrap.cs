using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    [SerializeField] protected int damage;
    protected Animator ani;
    protected IDamageType damageType;
    protected virtual void Start()
    {
        LoadComponent();
    }

    protected virtual void Update()
    {
        
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {

    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

    }
    protected virtual void LoadComponent()
    {
        this.ani = GetComponent<Animator>();
    }
}
