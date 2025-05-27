using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponInformation : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInfor
    {
        public TextMeshProUGUI name;
        public Image image;
    }
    public List<WeaponInfor> weapons = new List<WeaponInfor>();
    public GameObject changeWeaponUI;
    private WeaponManager weaponManager;
    private bool isActive ;
    private void Start()
    {
        this.weaponManager = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponentInChildren<WeaponManager>();
        EventManager.OP_EventManager.Subscribe("LoadInForWeapon", LoadInForWeapon);
        EventManager.OP_EventManager.Subscribe("Hide", Hide);
        EventManager.OP_EventManager.Subscribe("Show", Show);
    }
    private void Update()
    {
        HideControll();
    }
    void HideControll()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isActive && this.weaponManager.Weaponlist.Count == 2)
                this.Show();
            else
                this.Hide();
        }
        if (Input.GetKeyDown(KeyCode.X) && isActive)
        {
            this.weaponManager.SwapWeaponSlots();
        }
    }    
    void LoadInForWeapon()
    {
        StartCoroutine(DelayLoad());           
    }    
    IEnumerator DelayLoad()
    {
        yield return null;
        for (int i = 0; i < this.weaponManager.Weaponlist.Count; i++)
        {
            if (this.weaponManager.Weaponlist[i] != null)
            {
                WeaponBase weaponBase = this.weaponManager.Weaponlist[i].GetComponentInChildren<WeaponBase>();
                if (weaponBase != null)
                {
                    this.weapons[i].image.sprite = weaponBase.SrOriginal;
                    this.weapons[i].name.text = weaponBase.NameWeapon;
                }
            }
        }
    }
    void Hide()
    {       
        this.changeWeaponUI.SetActive(false);
        this.isActive = false;
        TimeManager.TimeResume();
    }
    void Show()
    {      
        this.changeWeaponUI.SetActive(true);
        this.isActive = true;
        TimeManager.TimePause();
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("LoadInForWeapon", LoadInForWeapon);
        EventManager.OP_EventManager.Unsubscribe("Hide", Hide);
        EventManager.OP_EventManager.Unsubscribe("Show", Show);
    }
}
