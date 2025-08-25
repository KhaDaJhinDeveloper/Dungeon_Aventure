using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/DialogData")]
public class DialogData : ScriptableObject
{
    public Sprite characterSprite;
    public string characterName;
    public string dialogID;
    public DialogLine[] dialogLines;
    public int currentLine;
    public bool canRepeat = false;
    
}
