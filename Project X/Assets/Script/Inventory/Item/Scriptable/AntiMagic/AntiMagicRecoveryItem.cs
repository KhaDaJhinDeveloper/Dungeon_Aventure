using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/AntiMagicRecoveryItem")]
public class AntiMagicRecoveryItem : RecoveryItem
{
    public int antiMagicAmount;
    public float additionalTime;
    public override void ApplyRecovery(PlayerStats player, CountdownTimer countdownTimer)
    {
        player.AntiMagicRecovery(this.antiMagicAmount);
        countdownTimer.IncreaseTime(this.additionalTime);
    }
}
