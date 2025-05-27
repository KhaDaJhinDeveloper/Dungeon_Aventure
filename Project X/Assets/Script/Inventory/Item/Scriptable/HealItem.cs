using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/HealItem")]
public class HealItem : RecoveryItem
{
    public int HealAmount;
    public override void ApplyRecovery(PlayerStats player)
    {
        if (player.CurentHealth < player.MaxHealth)
        {
            
            player.Healing(this.HealAmount);
        }
        else Debug.Log("da dây mau,, khong the dung");
    }
}
