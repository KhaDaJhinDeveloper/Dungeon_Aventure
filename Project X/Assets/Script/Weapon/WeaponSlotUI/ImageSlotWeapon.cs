using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlotWeapon : MonoBehaviour
{
    public List<GameObject> slotWeapon = new List<GameObject>();
    private bool[] slotFull;
    private Image[] imageWeapons;
    private WeaponManager weaponManager;
    private void Start()
    {
        this.weaponManager = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponentInChildren<WeaponManager>();
        this.slotFull = new bool[this.slotWeapon.Count];
        this.imageWeapons = new Image[this.slotWeapon.Count];
        for (int i = 0; i < this.slotWeapon.Count; i++)
        {
            this.imageWeapons[i] = this.slotWeapon[i].GetComponent<Image>();
        }
        EventManager.OP_EventManager.Subscribe<Sprite>("UpdateImageWeapon", UpdateImageWeapon);
        EventManager.OP_EventManager.Subscribe("LoadImageWeapon", LoadImageWeapon);
    }
    public void UpdateImageWeapon(Sprite imageWeapon)
    {    
        for (int i = 0; i < this.slotWeapon.Count; i++)
        {
            if (this.slotFull[i] == false)
            {
                imageWeapons[i].sprite = imageWeapon;
                slotFull[i] = true;
                break;
            }
            else continue;
        }  
    }
    public void LoadImageWeapon()
    {
        StartCoroutine(DelayLoad());
    }
    IEnumerator DelayLoad()
    {
        yield return null;
        for (int i = 0; i < this.weaponManager.Weaponlist.Count; i++)
        {
            WeaponBase weapon = this.weaponManager.Weaponlist[i].GetComponentInChildren<WeaponBase>();
            if (weapon != null)
            {
                imageWeapons[i].sprite = weapon.SrOriginal;
            }
        }
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe<Sprite>("UpdateImageWeapon", UpdateImageWeapon);
        EventManager.OP_EventManager.Unsubscribe("LoadImageWeapon", LoadImageWeapon);
    }
}
