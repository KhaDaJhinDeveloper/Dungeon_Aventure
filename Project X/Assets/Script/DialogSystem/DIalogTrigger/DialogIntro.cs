using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogIntro : BaseInteraction
{
    [SerializeField] private NameScene _nameScene;
    [SerializeField] private DialogData dialogData;
    private DialogManager dialogManager;
    protected override void LoadComponent()
    {
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    public void StartIntro()
    {
        if (!DialogStateManager.dialogState_Instance.IsDialogCompleted(this.dialogData.dialogID))
            this.dialogManager.StartDialogBox(this.dialogData);
        else
            SceneManager.LoadScene(this._nameScene.ToString());
    }
}
