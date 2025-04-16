using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{   
    //---Component
    private SpriteRenderer sr;
    private ImageSlotWeapon WeaponSlot;
    private WeaponManager weaponManager;
    private string targetName;
    //---Information Weapon
    bool canLoot = false;
    private Sprite defaultSprite;
    public Sprite DefaultSprite
    {
        get => defaultSprite;
    }
    void Start()
    {
        this.weaponManager = GameObject.FindWithTag(TagManager.TAG_WEAPONSLOTS_ATTACK).GetComponentInChildren<WeaponManager>();
        this.WeaponSlot = GameObject.Find("ImageSlotsWeapon").GetComponent<ImageSlotWeapon>();
        this.sr = GetComponent<SpriteRenderer>();
        this.defaultSprite = this.sr.sprite;
        this.targetName = this.gameObject.name;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canLoot)
        {
            Loot();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.canLoot = true;
            EventManager.OP_EventManager.TriggerEvent("EventShowButton");
        }         
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        { 
            this.canLoot = false;
            EventManager.OP_EventManager.TriggerEvent("EventHiddenButton");
        }           
    }
    void Loot()
    {
        if (!this.WeaponSlot.CheckSlots())
        {       
            EventManager.OP_EventManager.TriggerEvent<Sprite>("UpdateImageWeapon", this.defaultSprite);
            foreach (GameObject weapon in this.weaponManager.weaponPrefab)
            {
                if (weapon.name == targetName)
                {
                    this.weaponManager.AddWeaponList(weapon.gameObject);
                    this.gameObject.SetActive(false);
                }
            }          
        }
    }
}
