using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    //---Component
    private SpriteRenderer sr;
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
        this.sr = GetComponent<SpriteRenderer>();
        this.defaultSprite = this.sr.sprite;
        this.targetName = KeyClean.CleanKey(this.gameObject.name);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canLoot)
        {
            Loot();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.canLoot = true;
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.canLoot = false;
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
        }
    }
    void Loot()
    {
        if (!this.weaponManager.CheckCondition())
        {           
            foreach (GameObject weapon in this.weaponManager.weaponPrefab)
            {
                if (weapon.name == targetName )
                {
                    this.weaponManager.AddWeaponList(weapon);
                    EventManager.OP_EventManager.TriggerEvent<Sprite>("UpdateImageWeapon", this.defaultSprite);
                    this.gameObject.SetActive(false);
                }
            }
        }
        else if (this.weaponManager.CheckCondition())
        {
            if (this.weaponManager.WeaponReserve == null)
            {
                this.weaponManager.AddWeaponReserve(this.gameObject);
                EventManager.OP_EventManager.TriggerEvent("Show");
                this.gameObject.SetActive(false);
            }
            else
                DebugLogger.Log("Weapon reserve is full");
        }
    }
}
