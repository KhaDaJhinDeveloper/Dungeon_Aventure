using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/ArmorRecoveryItem")]
public class ArmorRecoveryItem : RecoveryItem
{
    public int armorAmount;
    public override void ApplyRecovery(PlayerStats player)
    {
        player.ArmorRecovery(this.armorAmount);
    }
}
