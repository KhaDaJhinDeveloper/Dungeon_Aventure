using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    private ListButtonChoices listButtonChoices;
    private DialogData currentDialog;
    private int currentDialogLineIndex = 0;
    private bool waitingForChoice;
    private bool isTying = false;
    public GameObject buttonNextDialog;
    public DialogData CurrentDialog { get => currentDialog;}
    public bool WaitingForChoice { get => waitingForChoice;}

    void Start()
    {
        this.listButtonChoices = GetComponentInChildren<ListButtonChoices>();
        DialogStateManager.dialogState_Instance.LoadDialogState();
        CloseDialogBox();
    }
    private void Update()
    {
        HideButtonNextDialog();
    }
    public void StartDialogBox(DialogData dialogdata, int indexDialog = 0)
    {
        OpenDialogBox();
        this.currentDialog = dialogdata;
        this.currentDialogLineIndex = indexDialog;
        this.waitingForChoice = false;       
        DisplayCurrentLine();
    }    

    public void DisplayCurrentLine()
    {
        if (this.currentDialog == null || this.currentDialogLineIndex >= this.currentDialog.dialogLines.Length)
        {
            Debug.Log("thoa man");
            EndDialogBox();
            return;        
        }
        DialogLine currentLine = this.currentDialog.dialogLines[this.currentDialogLineIndex];
        if (!string.IsNullOrEmpty(currentLine.requiredFlag) && !DialogStateManager.dialogState_Instance.HasFlag(currentLine.requiredFlag))
        {
            this.currentDialogLineIndex++;
            DisplayCurrentLine();
            return;
        }
        if (!string.IsNullOrEmpty(currentLine.setFlag))
        {
            DialogStateManager.dialogState_Instance.SetFlag(currentLine.setFlag);
        }
        if (currentLine.hasEvent && currentLine.executeEventOnStart)
            DialogEventManager.dialogEvent_Instance.CallDialogEvent(currentLine.dialogEvents);
        DisplayTextDialog(currentLine);
        ShowChoices(currentLine.choices);
    }    
    public void ShowChoices(DialogChoice[] choices)
    {
        if(choices.Length > 0) this.waitingForChoice = true;
        this.listButtonChoices.ActiveButton(choices.Length);
        for(int i=0; i < choices.Length; i++)
        {
            if (!string.IsNullOrEmpty(choices[i].requiedFlag) && !DialogStateManager.dialogState_Instance.HasFlag(choices[i].requiedFlag))
                continue;
            TextMeshProUGUI choicesText = this.listButtonChoices.arrayButtonChoices[i].GetComponentInChildren<TextMeshProUGUI>();
            choicesText.text = choices[i].choiceText;
            DialogChoice choiceRef = choices[i];
            Button choicesButton = this.listButtonChoices.arrayButtonChoices[i].GetComponent<Button>();
            choicesButton.onClick.AddListener(() => OnChoiceSelected(choiceRef));
        }    
    }
    public void CompleteCurrentLine()
    {
        DialogLine curentline = this.currentDialog.dialogLines[this.currentDialogLineIndex];
        if (curentline.hasChoice)
        {
            ShowChoices(curentline.choices);
        }
    }
    public void ProcessCurrentLine()
    {
        DialogLine currentLine = this.currentDialog.dialogLines[this.currentDialogLineIndex];
        if (currentLine.hasEvent && currentLine.executeEventOnStart)
            DialogEventManager.dialogEvent_Instance.CallDialogEvent(currentLine.dialogEvents);
        if (currentLine.hasChoice) return;
        if(currentLine.nextDialogIndex >= 0)
        {
            this.currentDialogLineIndex = currentLine.nextDialogIndex;
            DisplayCurrentLine();
        }
        else
        {
            this.currentDialogLineIndex++;
            DisplayCurrentLine();
        }
    }
    public void OnChoiceSelected(DialogChoice choice)
    {
        this.waitingForChoice = false;
        if(!string.IsNullOrEmpty(choice.setFlag))
        {
            DialogStateManager.dialogState_Instance.SetFlag(choice.setFlag);
        }
        if(choice.nextDialogIndex >= 0)
        {
            this.currentDialogLineIndex = choice.nextDialogIndex;
            this.listButtonChoices.HideButton();
            DisplayCurrentLine();
        }
        else EndDialogBox();
    }
    public void EndDialogBox()
    {
        if (this.currentDialog != null && !string.IsNullOrEmpty(this.currentDialog.dialogID))
        {
            DialogStateManager.dialogState_Instance.MarkDialogCompleted(this.currentDialog.dialogID);
        }    
        this.listButtonChoices.HideButton();
        DialogStateManager.dialogState_Instance.UpdateProgress(this.currentDialog.dialogID, this.currentDialogLineIndex);
        this.currentDialog = null;
        CloseDialogBox();
    }    
    public void OnNextButtonClicked()
    {
        if (this.waitingForChoice) return;
        if (this.isTying) CompleteCurrentLine();
        else ProcessCurrentLine();
    }
    void HideButtonNextDialog()
    {
        if(this.waitingForChoice)
            this.buttonNextDialog.SetActive(false);
        else
            this.buttonNextDialog.SetActive(true);
    }
    public void DisplayTextDialog(DialogLine currentLine) => StartCoroutine(ShowTextDialog(currentLine));
    IEnumerator ShowTextDialog(DialogLine currentLine)
    {
        this.isTying = true;
        yield return null;
        EventManager.OP_EventManager.TriggerEvent<string>(NameEvent.Event_LoadDialogText, currentLine.dialogText);
        this.isTying = false;
    }
    public void CloseDialogBox() => this.dialogBox.SetActive(false);
    public void OpenDialogBox()  => this.dialogBox.SetActive(true);
}    
