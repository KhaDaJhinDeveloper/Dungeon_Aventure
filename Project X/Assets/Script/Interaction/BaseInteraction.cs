using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteraction : MonoBehaviour
{
    public bool canInteract = true;
    protected virtual void Awake()
    {
        LoadComponent();
    }
    protected virtual void Start()
    {

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

    }
    public virtual void DisableObject()
    {

    }
    public virtual void OnLoadData(bool canInteract)
    { }
}
