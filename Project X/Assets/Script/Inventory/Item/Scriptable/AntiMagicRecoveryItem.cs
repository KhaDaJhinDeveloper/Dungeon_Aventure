using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/AntiMagicRecoveryItem")]
public class AntiMagicRecoveryItem : RecoveryItem
{
    public int antiMagicAmount;
    public override void ApplyRecovery(PlayerStats player)
    {
        player.AntiMagicRecovery(this.antiMagicAmount);
    }
}
