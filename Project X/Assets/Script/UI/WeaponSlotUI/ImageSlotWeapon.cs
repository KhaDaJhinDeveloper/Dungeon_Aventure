using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlotWeapon : MonoBehaviour
{
    private WeaponController weaponController;
    [SerializeField] private Image imageWeapon01;
    [SerializeField] private Image imageWeapon02;
    [SerializeField] private Image imageWeaponReserve;
    [SerializeField] private Sprite weaponNull;
    private void LoadImageSlotWeapon()
    {
        if (this.weaponController == null)
            this.weaponController = FindFirstObjectByType<WeaponController>();
        this.imageWeapon01.sprite = this.weaponController.weaponsArray[0].isFull == true? this.weaponController.weaponsArray[0].currentWeapon.spriteWeapon : weaponNull;
        this.imageWeapon02.sprite = this.weaponController.weaponsArray[1].isFull == true ? this.weaponController.weaponsArray[1].currentWeapon.spriteWeapon : weaponNull;
        this.imageWeaponReserve.sprite = this.weaponController.weaponReserve.isFull == true? this.weaponController.weaponReserve.currentWeapon.spriteWeapon : weaponNull;
    }
    private void OnEnable()
    {
        EventManager.Instance?.Subscribe(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI, LoadImageSlotWeapon);
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI, LoadImageSlotWeapon);
    }
}
