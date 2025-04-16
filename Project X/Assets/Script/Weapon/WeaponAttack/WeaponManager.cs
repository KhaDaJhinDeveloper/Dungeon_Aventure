using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weaponlist = new List<GameObject>();
    public List<GameObject> weaponPrefab = new List<GameObject>();
    private IWeapon[] attackComponent = new IWeapon[2];
    private bool isAttacking = true;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    void Update()
    {
        AttackInput();
    }
    void AttackInput()
    {
        if (Input.GetMouseButtonDown(0) && this.isAttacking)
        {
            if(this.attackComponent[0] != null)
            { 
                this.isAttacking = false;
                this.attackComponent[0].WeaponAttack();
            }             
        }
        else if (Input.GetMouseButtonDown(1) && this.isAttacking)
        {
            if (this.attackComponent[1] != null)
            {
                this.isAttacking = false;
                this.attackComponent[1].WeaponAttack();
            }
        }
    }
    public void ChangeWeaponSlot1(GameObject weapon)
    {
        this.weaponlist[0] = weapon;
    }
    public void ChangeWeaponSlot2(GameObject weapon)
    {
        this.weaponlist[1] = weapon;
    }
    public void SwapWeaponSlots()
    {
        GameObject obj = this.weaponlist[0];
        this.weaponlist[0] = this.weaponlist[1];
        this.weaponlist[1] = obj;
    }
    public void AddWeaponList(GameObject weapon)
    {
        if (!this.weaponlist.Contains(weapon) && this.weaponlist.Count < 2)
        {
            this.weaponlist.Add(weapon);
            weapon.SetActive(true);
            GetInterfaceWeapon();
        }
    }   
    public void GetInterfaceWeapon()
    {
        for (int i = 0; i < weaponlist.Count; i++)
        {
            if (weaponlist[i] != null)
                this.attackComponent[i] = weaponlist[i].GetComponentInChildren<IWeapon>();
            else
                this.attackComponent[i] = null;
        }
    }
}
