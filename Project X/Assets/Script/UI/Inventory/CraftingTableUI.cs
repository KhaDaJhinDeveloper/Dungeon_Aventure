using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTableUI : MonoBehaviour
{
    [SerializeField] private Sprite spriteDefault;
    [SerializeField] private Image[] imageSlot;
    [SerializeField] private Image ImageCompleteEquipment;
    private CraftingItem craftingItem;
    void Start()
    {
        this.craftingItem = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<CraftingItem>();
        EventManager.OP_EventManager.Subscribe("LoadCraftingeUI", LoadCraftingeUI);
        EventManager.OP_EventManager.Subscribe("LoadUIDefault", EmptySlot);
    }
    void LoadCraftingeUI()
    {
        for(int i = 0; i < this.craftingItem.RawMaterials.Length; i++)
        {
            if (this.craftingItem.RawMaterials[i] != null)
            {
                this.imageSlot[i].sprite = craftingItem.RawMaterials[i].ImageItem.sprite;
            }
        }
        if(this.ImageCompleteEquipment.sprite != null)
            this.ImageCompleteEquipment.sprite = this.craftingItem.CompleteEquipment.ImageItem.sprite;
    }
    void EmptySlot()
    {
        for (int i = 0; i < this.craftingItem.RawMaterials.Length; i++)
        {
            this.imageSlot[i].sprite = this.spriteDefault;
        }
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("LoadCraftingeUI", LoadCraftingeUI);
        EventManager.OP_EventManager.Unsubscribe("LoadUIDefault", EmptySlot);
    }
}
