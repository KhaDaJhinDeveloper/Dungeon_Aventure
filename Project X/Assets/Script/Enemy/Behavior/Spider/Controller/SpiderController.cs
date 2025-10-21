using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpiderController : MonoBehaviour
{
    [Header("Components")]
    public GameObject sprite;
    public Animator ani;
    public Rigidbody2D rb;
    public BaseStats stats;
    public GameObject targetObject;
    [Header("Movement")]
    public int speedMove;
    [Header("Detec")]
    public bool isDetecPlayer;
    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void Update()
    {
        
    }    
    protected virtual void LoadComponent()
    {
        this.stats = GetComponent<BaseStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.targetObject = GameObject.FindWithTag(TagManager.TAG_PLAYER);
        this.speedMove = this.stats.Speed;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
