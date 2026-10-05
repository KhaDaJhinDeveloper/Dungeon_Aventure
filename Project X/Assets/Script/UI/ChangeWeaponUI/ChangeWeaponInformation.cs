using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeWeaponInformation : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInfor
    {
        public TextMeshProUGUI name;
        public Image image;
    }
    public WeaponInfor[] weapons = new WeaponInfor[2];
    [SerializeField] private WeaponInfor weaponReverse;
    [SerializeField] private GameObject changeWeaponUI;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject background;
    [SerializeField] private Sprite spriteDefault;
    private WeaponController weaponControl;
    private bool isActive ;
    private void Start()
    {
        this.weaponControl = FindFirstObjectByType<WeaponController>();
        EventManager.Instance.Subscribe(NameEvent.Event_ChangeWeaponSlot_ShowUI, Show);
        EventManager.Instance.Subscribe(NameEvent.Event_ChangeWeaponSlot_HideUI, Hide);
        EventManager.Instance.Subscribe(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon, LoadInfoWeapon);
        this.changeWeaponUI.transform.position = this.startPos.position;
    }
    #region EventButton

    #endregion
    #region EventUI
    public void LoadInfoWeapon()
    {
        SetInfoWeapon(this.weapons[0], this.weaponControl.weaponsArray[0]);
        SetInfoWeapon(this.weapons[1], this.weaponControl.weaponsArray[1]);
        SetInfoWeapon(this.weaponReverse, this.weaponControl.weaponReserve);
    }
    public void SetInfoWeapon(WeaponInfor info,WeaponSlot weaponSlot)
    {
        if (weaponSlot.isFull)
        {
            info.name.text = weaponSlot.currentWeapon.nameWeapon;
            info.image.sprite = weaponSlot.currentWeapon.spriteWeapon;
        }
        else
        {
            info.name.text = "";
            info.image.sprite = this.spriteDefault;
        }
    }
    void Hide()
    {       
        this.isActive = false;
        this.changeWeaponUI.transform.DOKill();
        this.changeWeaponUI.transform.DOMove(this.startPos.transform.position, 0.5f).SetUpdate(true).OnComplete(() => {this.background.SetActive(false);
                                                                                                                       this.changeWeaponUI.SetActive(false);
                                                                                                                       TimeManager.TimeResume();
                                                                                                                       this.weaponControl.AllowTheAttack();});                                                                                                                                                                                                                                                                 
    }
    void Show()
    {      
        this.isActive = true;
        this.background.SetActive(true);
        this.changeWeaponUI.SetActive(true);
        this.changeWeaponUI.transform.DOLocalMove(new Vector3(0, 30, 0), 0.5f).SetUpdate(true);
        this.weaponControl.BlockAttack();
        TimeManager.TimePause();
    }
    #endregion
    private void OnDestroy()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Unsubscribe(NameEvent.Event_ChangeWeaponSlot_ShowUI, Show);
        EventManager.Instance.Unsubscribe(NameEvent.Event_ChangeWeaponSlot_HideUI, Hide);
        EventManager.Instance.Unsubscribe(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon, LoadInfoWeapon);
    }
}
