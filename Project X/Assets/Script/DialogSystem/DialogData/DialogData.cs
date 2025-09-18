using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/DialogData")]
public class DialogData : ScriptableObject
{
    public Sprite characterSprite;
    public string unlockFlag;
    public string characterName;
    public string dialogID;
    public DialogLine[] dialogLines;
    //Important Conditions
    public bool hasunlockFlag;
    public bool canRepeat;   
}
