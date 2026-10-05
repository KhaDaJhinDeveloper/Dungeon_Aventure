using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour,IWeapon
{
    public WeaponInstance currentWeapon { get; private set; }
    private WeaponController weaponController;
    private Animator weaponAni;
    private PlayerStats playerStats;
    public bool isFull;
    private void Start()
    {
        this.playerStats = FindFirstObjectByType<PlayerStats>();
        this.weaponController = FindFirstObjectByType<WeaponController>();
        this.weaponAni = GetComponent<Animator>();
    }
    #region SlotInteract
    public void SetWeapon(WeaponInstance weaponInstance)
    {
        if (this.weaponAni == null)
            this.weaponAni = GetComponent<Animator>();

        this.currentWeapon = weaponInstance;
        if (this.currentWeapon != null && this.currentWeapon.baseData != null)
        {
            this.weaponAni.runtimeAnimatorController = this.currentWeapon.baseData.animatorController;
            this.isFull = true;
            this.gameObject.SetActive(true);
        }
    }
    public void ClearSlot()
    {
        this.currentWeapon = null;
        this.weaponAni.runtimeAnimatorController = null;
        this.isFull = false;
        this.gameObject.SetActive(false);
        this.weaponController.AllowTheAttack();
    }
    #endregion
    #region Attack
    public void WeaponAttack()
    {
        if(this.playerStats == null) this.playerStats = FindFirstObjectByType<PlayerStats>();
        if (this.currentWeapon.isUsedMana )
        {
            if (this.playerStats.Mana >= this.currentWeapon.manaQty)
            {
                this.weaponAni.SetTrigger("attack");
                this.playerStats.ManaReduce(this.currentWeapon.manaQty);
                return;
            }
            else
            {
                this.weaponController.AllowTheAttack();
                return;
            }
                //SoundWarning not enought mana
            }
            this.weaponAni.SetTrigger("attack");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
        {
            BaseStats objEnemy = collision.gameObject.GetComponentInChildren<BaseStats>();
            DealDamage(objEnemy);
        }    
    }

    public void Shooting()
    {
        ShootPattern shootMethod = this.currentWeapon.baseData.shootPattern;
        shootMethod?.AttackShoot(this.currentWeapon, this.transform);
    }
    public void DealDamage(BaseStats stats)
    {
        if (this.currentWeapon == null || stats == null) return;
        stats.TakeDamage(this.currentWeapon.GetOppositeMainDamage(), this.currentWeapon.damageType,stats.transform);
    }
    public void AllowTheAttack() => this.weaponController.AllowTheAttack();
    #endregion
}
