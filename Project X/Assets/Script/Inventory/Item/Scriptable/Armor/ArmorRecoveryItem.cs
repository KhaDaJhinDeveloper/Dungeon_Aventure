using UnityEngine;

[CreateAssetMenu(menuName = "Items/ArmorRecoveryItem")]
public class ArmorRecoveryItem : RecoveryItem
{
    public int armorAmount;
    public float additionalTime;
    public override void ApplyRecovery(PlayerStats player, CountdownTimer countdownTimer)
    {
        player.ArmorRecovery(this.armorAmount);
        countdownTimer.IncreaseTime(this.additionalTime);
    }
}
