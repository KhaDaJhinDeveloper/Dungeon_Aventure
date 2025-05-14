using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] protected string nameitem;
    [SerializeField] protected Sprite imageItem;
    [SerializeField] protected int amount;
    protected virtual void Start()
    {
        this.LoadIndexItem();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void LoadIndexItem()
    {

    }    
}
