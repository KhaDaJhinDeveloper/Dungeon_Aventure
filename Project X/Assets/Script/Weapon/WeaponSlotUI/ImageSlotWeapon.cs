using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlotWeapon : MonoBehaviour
{
    public List<GameObject> slotWeapon = new List<GameObject>();
    private int numberSlots;
    private bool[] slotFull;
    private Image[] imageWeapons;

    private void Start()
    {
        this.numberSlots = this.slotWeapon.Count;
        this.slotFull = new bool[this.slotWeapon.Count];
        this.imageWeapons = new Image[this.slotWeapon.Count];
        for (int i = 0; i < this.slotWeapon.Count; i++)
        {
            this.imageWeapons[i] = this.slotWeapon[i].GetComponent<Image>();
        }
        EventManager.OP_EventManager.Subscribe<Sprite>("UpdateImageWeapon", UpdateImageWeapon);
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
    public bool CheckSlots()
    {
        foreach(bool isFull in this.slotFull)
        {
            if (!isFull) return false;
        }
        return true;
    }
}
