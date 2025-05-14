using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemArmorRecoverySO : ScriptableObject
{
    public int amount;
    public string nameItem;
    public void UseItem()
    {
        GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>().ArmorRecovery(this.amount);
    }    
}
