using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private GameObject obj;
    void Start()
    {
        this.playerStats = GetComponent<PlayerStats>();
        this.rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        float inputLeftRight = Input.GetAxisRaw("Horizontal");
        float inputUpDown = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(inputLeftRight * this.playerStats.Speed, inputUpDown * this.playerStats.Speed);
    }
}
