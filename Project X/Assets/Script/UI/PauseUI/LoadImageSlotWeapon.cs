using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadImageSlotWeapon : MonoBehaviour
{
    public Image[] imageSlotWeapon;
    private ChangeWeaponInformation changeWeaponInformation;
    void Start()
    {
        this.changeWeaponInformation = GameObject.FindFirstObjectByType<ChangeWeaponInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        //Load();
    }
    void Load()
    {
        for(int i = 0; i < imageSlotWeapon.Length; i++)
        {
            this.imageSlotWeapon[i].sprite = changeWeaponInformation.weapons[i].image.sprite;
        }    
    }
}
