using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private ListButtonChoices listButtonChoices;
    private DialogData currentDialog;
    public DialogData defaultDialog;
    private int currentDialogLineIndex = 0;
    private bool activeDialog;
    private bool waitingForChoice;
    private bool isTying = false;

    public DialogData CurrentDialog { get => currentDialog;}
    void Start()
    {
        this.listButtonChoices = GetComponentInChildren<ListButtonChoices>();
        StartDialogBox(this.defaultDialog);   
        //CloseDialogBox();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.currentDialogLineIndex++;
            DisplayCurrentLine();
        }
    }
    public void StartDialogBox(DialogData dialogdata)
    {
        OpenDialogBox();
        this.currentDialog = dialogdata;
        this.currentDialogLineIndex = 0;
        this.activeDialog = true;
        this.waitingForChoice = false;       
        DisplayCurrentLine();
    }    

    void DisplayCurrentLine()
    {
        if (this.currentDialog == null /*|| this.currentDialogLineIndex >= this.currentDialog.dialogLines.Length*/)
        {
            return;
        }
        DialogLine currentLine = this.currentDialog.dialogLines[this.currentDialogLineIndex];
        DisplayTextDialog(currentLine);
        if (!string.IsNullOrEmpty(currentLine.setFlag))
        {
            DialogStateManager.dialogState_Instance.SetFlag(currentLine.setFlag);
        }
        if (!string.IsNullOrEmpty(currentLine.requiredFlag) && !DialogStateManager.dialogState_Instance.HasFlag(currentLine.requiredFlag))
        {
            this.currentDialogLineIndex++;
            DisplayCurrentLine();
            return;
        }
        DisplayTextDialog(currentLine);
        ShowChoices(currentLine.choices);
    }    
    public void CompleteCurrentLine()
    {
        DialogLine curentline = this.currentDialog.dialogLines[this.currentDialogLineIndex];
        if(curentline.hasChoice)
        {
            ShowChoices(curentline.choices);
        }    
    }    
    public void ShowChoices(DialogChoice[] choices)
    {
        this.waitingForChoice = true;
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
    public void ProcessCurrentLine()
    {
        DialogLine currentLine = this.currentDialog.dialogLines[this.currentDialogLineIndex];
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
            DisplayCurrentLine();
            this.listButtonChoices.HideButton();
        }
        else EndDialogBox();
    }
    public void EndDialogBox()
    {
        if(this.currentDialog != null && !string.IsNullOrEmpty(this.currentDialog.dialogID))
        {
            DialogStateManager.dialogState_Instance.MarkDialogCompleted(this.currentDialog.dialogID);
        }    
        this.currentDialog = null;
        CloseDialogBox();
    }    
    public void OnNextButtonClicked()
    {
        if (this.waitingForChoice) return;
        if (this.isTying) CompleteCurrentLine();
        else ProcessCurrentLine();
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
