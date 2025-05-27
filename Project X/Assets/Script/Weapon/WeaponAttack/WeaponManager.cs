using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<GameObject> weaponlist = new List<GameObject>();
    public List<GameObject> weaponPrefab = new List<GameObject>();
    private IWeapon[] attackComponent = new IWeapon[2];
    private GameObject weaponReserve;
    private bool isAttacking = true;
    private Transform posDrop;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<GameObject> Weaponlist { get => weaponlist; set => weaponlist = value; }
    public GameObject WeaponReserve { get => weaponReserve; set => weaponReserve = value; }
    private void Start()
    {
        this.posDrop = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<Transform>();
        EventManager.OP_EventManager.Subscribe("ChangeWeaponSlot1", ChangeWeaponSlot1);
        EventManager.OP_EventManager.Subscribe("ChangeWeaponSlot2", ChangeWeaponSlot2);
    }
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
    void ChangeWeaponSlot1()
    {
        if(this.weaponReserve != null)
        {
            GameObject obj = this.weaponlist[0];
            this.weaponlist[0] = this.weaponReserve;
            this.weaponReserve = obj;
            this.weaponlist[0].SetActive(true);          
            GetInterfaceWeapon();
            LoadUI();
            DropWeapon(this.weaponReserve);
        }

    }
    void ChangeWeaponSlot2()
    {
        if (this.weaponReserve != null)
        {
            GameObject obj = this.weaponlist[1];
            this.weaponlist[1] = this.weaponReserve;
            this.weaponReserve = obj;
            this.weaponlist[1].SetActive(true);
            GetInterfaceWeapon();
            LoadUI();
            DropWeapon(this.weaponReserve);
        }
    }
    public void SwapWeaponSlots()
    {
        GameObject obj = this.weaponlist[0];
        this.weaponlist[0] = this.weaponlist[1];
        this.weaponlist[1] = obj;
        GetInterfaceWeapon();
        LoadUI();
    }
    public void AddWeaponList(GameObject weapon)
    {
        if (this.weaponlist.Count < 2)
        {
            this.weaponlist.Add(weapon);
            weapon.SetActive(true);
            GetInterfaceWeapon();            
        }
        else
            EventManager.OP_EventManager.TriggerEvent("Show");
    }   
    public void AddWeaponReserve(GameObject weapon)
    {
        if (this.weaponReserve == null)
        {
            foreach (GameObject weaponPb in this.weaponPrefab)
            {
                if (weaponPb.name == weapon.name || weapon.name == weaponPb.name + "(Clone)")
                {
                    this.weaponReserve = weaponPb;
                }
            }
        }
    }    
    public void GetInterfaceWeapon()
    {
        for (int i = 0; i < weaponlist.Count; i++)
        {
            if (weaponlist[i] != null )
                this.attackComponent[i] = weaponlist[i].GetComponentInChildren<IWeapon>();              
            else
                this.attackComponent[i] = null;
        }
        LoadUI();
    }
    public bool CheckCondition()
    {
        if(this.weaponlist.Count >= 2 )
            return true; 
        return false;
    } 
    public void DropWeapon(GameObject obj)
    {
        if(this.weaponReserve != null)
        {   
            GameObject weaponDrop = ObjectPooling.ObjectPooling_Instance.GetPool(obj.name);
            weaponDrop.transform.position = this.posDrop.transform.position;
            this.weaponReserve = null;
        }    
    }    
    void LoadUI()
    {
        EventManager.OP_EventManager.TriggerEvent("LoadInForWeapon");
        EventManager.OP_EventManager.TriggerEvent("LoadImageWeapon");
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("ChangeWeaponSlot1", ChangeWeaponSlot1);
        EventManager.OP_EventManager.Unsubscribe("ChangeWeaponSlot2", ChangeWeaponSlot2);
    }
}
