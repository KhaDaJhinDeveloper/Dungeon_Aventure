using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    #region Input
    [SerializeField] private WeaponData weaponData;
    public WeaponInstance weaponInstance {  get; private set; }
    public KeyPool Key;
    #endregion
    bool canLoot = false;
    private void OnEnable()
    {
        Initialize(this.weaponData);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canLoot)
        {
            Loot();
        }
    }
    public void Initialize(WeaponData weaponData)
    {
        this.weaponInstance = new WeaponInstance(weaponData);
    }
    public void Initialize(WeaponInstance weaponInstance)
    {
        this.weaponInstance = weaponInstance;
    }
    #region Collider Event
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
        WeaponController controller = FindFirstObjectByType<WeaponController>();
        if (controller != null)
        {
            if (!controller.WeaponSlotIsFull())
            {
                controller.EquipWeapon(this.weaponInstance);
                ObjectPooling.Instance.ReturnToPool(this.Key, this.gameObject);
            } 
            else EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_ShowUI);
        }
    }
    #endregion
}
