using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/HealItem")]
public class HealItem : RecoveryItem
{
    public int HealAmount;
    public float additionalTime;
    public override void ApplyRecovery(PlayerStats player, CountdownTimer countdownTimer)
    { 
        player.Healing(this.HealAmount);
        countdownTimer.IncreaseTime(this.additionalTime);
    }
}
