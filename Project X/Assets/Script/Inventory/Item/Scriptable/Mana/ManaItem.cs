using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/ManaItem")]
public class ManaItem : RecoveryItem
{
    public int ManaAmount;
    public float additionalTime;
    public override void ApplyRecovery(PlayerStats player, CountdownTimer countdownTimer)
    {
        player.ManaRecovery(this.ManaAmount);
        countdownTimer.IncreaseTime(this.additionalTime);
    }
}
