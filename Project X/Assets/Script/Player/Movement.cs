using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    //public InputAction playerActionControls;
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    /*private Vector2 moveDirection = Vector2.zero;
    private void OnEnable()
    {
        playerActionControls.Enable();
    }
    private void OnDisable()
    {
        playerActionControls.Disable();
    }*/
    void Start()
    {
        this.playerStats = GetComponent<PlayerStats>();
        this.rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //moveDirection = playerActionControls.ReadValue<Vector2>();
        Move();
    }
    void Move()
    {
        float inputLeftRight = Input.GetAxisRaw("Horizontal");
        float inputUpDown = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(inputLeftRight * this.playerStats.Speed, inputUpDown * this.playerStats.Speed);
    }
}
