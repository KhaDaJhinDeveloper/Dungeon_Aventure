using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private Transform posDrop;
    public WeaponSlot[] weaponsArray;
    public WeaponSlot weaponReserve;
    private bool attack = false;
    private void OnEnable()
    {
        EventManager.Instance.Subscribe<int>(NameEvent.Event_WeaponControll_ChangeSlotWeapon, ChangeWeaponSlot);
        EventManager.Instance.Subscribe(NameEvent.Event_WeaponControll_SwapWeapon, SwapWeapon);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attack)
        {
            if(this.weaponsArray[0].isFull)
            {
                BlockAttack();
                this.weaponsArray[0].WeaponAttack();
            }
        }
        else if (Input.GetMouseButtonDown(1) && !attack)
        {
            if (this.weaponsArray[1].isFull)
            {
                BlockAttack();
                this.weaponsArray[1].WeaponAttack();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && SlotIsFull())
            EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_ShowUI);
    }
    #region SlotInteract
    public void EquipWeapon(WeaponInstance weapon)
    {
        foreach (var weaponSlot in this.weaponsArray)
        {
            if (weaponSlot.isFull == false)
            {
                weaponSlot.SetWeapon(weapon);
                EventManager.Instance.TriggerEvent(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI);
                EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon);
                return;
            }
        }
        this.weaponReserve.SetWeapon(weapon);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon);
        return;
    }
    public void ChangeWeaponSlot(int ID)
    {   if (!this.weaponReserve.isFull || ID > this.weaponsArray.Length) return;
        DropWeapon(this.weaponsArray[ID].currentWeapon);
        this.weaponsArray[ID].ClearSlot();
        this.weaponsArray[ID].SetWeapon(this.weaponReserve.currentWeapon);
        this.weaponReserve.ClearSlot();
        EventManager.Instance.TriggerEvent(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon);
    }
    private bool SlotIsFull()
    {
        foreach(var weapon in this.weaponsArray)
        {
            if(weapon.isFull == true)
                return true;
        }
        return false;
    }
    public bool WeaponSlotIsFull()=> SlotIsFull() && this.weaponReserve.isFull;
    public void SwapWeapon()
    {
        var temp = this.weaponsArray[0].currentWeapon;
        this.weaponsArray[0].SetWeapon(this.weaponsArray[1].currentWeapon);
        this.weaponsArray[1].SetWeapon(temp);
        this.AllowTheAttack();
        EventManager.Instance.TriggerEvent(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon);
    }
    #endregion
    #region AttackController
    public void BlockAttack() => this.attack = true;
    public void AllowTheAttack() => this.attack = false;
    #endregion
    #region Drop
    void DropWeapon(WeaponInstance weaponInstance)
    {
        KeyPool  key= weaponInstance.baseData.keyPool;
        WeaponItem weapon = ObjectPooling.Instance.GetPool(key)?.GetComponent<WeaponItem>();
        weapon.transform.position = this.posDrop.position;
        weapon.Initialize(weaponInstance);
    }
    #endregion
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_WeaponControll_ChangeSlotWeapon, ChangeWeaponSlot);
        EventManager.Instance?.Unsubscribe(NameEvent.Event_WeaponControll_SwapWeapon, SwapWeapon);
    }
}
