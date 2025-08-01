using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNameCharacter : BaseText
{
    private DialogManager dialogManager;
    protected override void Start()
    {
        base.Start();
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    void Update()
    {
        Load();
    }
    public override void Load()
    {
        if(this.dialogManager.CurrentDialog != null)
            m_TextMeshProUGUI.text = this.dialogManager.CurrentDialog.characterName;
    }
}
