using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecoveryItem : ScriptableObject
{
    public string nameItem;
    public Sprite spriteImage;
    public ItemType type;
    public abstract void ApplyRecovery(PlayerStats player);
}
