using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCharater : MonoBehaviour
{
    private Image imageCharacter;
    private DialogManager dialogManager;
    private void Start()
    {
        this.imageCharacter = GetComponent<Image>();
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    private void Update()
    {
        if(this.dialogManager.CurrentDialog != null)
            this.imageCharacter.sprite = this.dialogManager.CurrentDialog.characterSprite;
    }
}
