using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private PlayerAnimation playerAni;
    private WeaponDirection weaponControl;
    void Start()
    {
        this.playerStats = GetComponent<PlayerStats>();
        this.rb = GetComponent<Rigidbody2D>();
        this.playerAni = GetComponentInChildren<PlayerAnimation>();
        this.weaponControl = GetComponent<WeaponDirection>();
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        float inputLeftRight = Input.GetAxisRaw("Horizontal");
        float inputUpDown = Input.GetAxisRaw("Vertical");
        this.rb.velocity = new Vector2(inputLeftRight * this.playerStats.Speed, inputUpDown * this.playerStats.Speed);
        this.playerAni.UpdateAnimation(this.rb.velocity);
        this.weaponControl.UpdateWeaponDirection(this.rb.velocity);
    }
}
