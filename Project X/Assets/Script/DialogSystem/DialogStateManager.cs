using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStateManager : Singleton<DialogStateManager>
{
    public static DialogStateManager dialogState_Instance;
    public HashSet<string> completedDialogs = new HashSet<string>();
    public Dictionary<string, bool> dialogFlags = new Dictionary<string, bool>();
    public Dictionary<string, int> dialogProgress = new Dictionary<string, int>();
    public DialogSaveData dialogSaveData = new DialogSaveData();
    protected override void Awake()
    {
        dialogState_Instance = this;
    }
    public void Start()
    {
        LoadDialogState();
    }
    public void MarkDialogCompleted(string dialogID)
    {
        if(!string.IsNullOrEmpty(dialogID))
        {
            this.completedDialogs.Add(dialogID);
            SaveDialogState();
        }    
    }
    public bool CheckDialogCompleted(string dialogID)
    {
        return this.completedDialogs.Contains(dialogID);
    }
    public void UpdateProgress()
    {

    }    
    public void SetFlag(string flagName, bool value = true)
    {
        if(!string.IsNullOrEmpty(flagName))
        {
            this.dialogFlags[flagName] = value;
            SaveDialogState();
        }    
    }    
    public bool HasFlag(string flagName)
    {
        return this.dialogFlags.ContainsKey(flagName) && this.dialogFlags[flagName];
    }
    public void ResetAllStateDialog()
    {
        this.dialogFlags.Clear();
        this.completedDialogs.Clear();
        PlayerPrefs.DeleteAll();
        SaveDialogState();
    }
    public void SaveDialogState()
    {
        List<DialogFlags> flagsList = new List<DialogFlags>();
        List<DialogProgress> progressList = new List<DialogProgress>();
        foreach(var cp in this.dialogFlags)
        {
            flagsList.Add(new DialogFlags(cp.Key, cp.Value));
        }
        foreach(var cx in dialogProgress)
        {
            progressList.Add(new DialogProgress(cx.Key, cx.Value));
        }          
        this.dialogSaveData.completedDialogs = new List<string>(this.completedDialogs);
        this.dialogSaveData.dialogFlags = flagsList;
        this.dialogSaveData.dialogProgress = progressList;
        string json = JsonUtility.ToJson(this.dialogSaveData);
        PlayerPrefs.SetString("DialogState", json);
        PlayerPrefs.Save();
    }   
    public void LoadDialogState()
    {
        if(PlayerPrefs.HasKey("DialogState"))
        {
            string json = PlayerPrefs.GetString("DialogState");
            DialogSaveData saveData = JsonUtility.FromJson<DialogSaveData>(json);
            if(saveData.completedDialogs != null)
            {
                this.completedDialogs = new HashSet<string>(saveData.completedDialogs);
            }    
            else
                this.completedDialogs = new HashSet<string>();
            this.dialogFlags = new Dictionary<string, bool>();
            if(this.dialogSaveData.dialogFlags != null)
            {
                foreach (DialogFlags flag in saveData.dialogFlags)
                {
                    if(!string.IsNullOrEmpty(flag.key))
                    {
                        this.dialogFlags[flag.key] = flag.value;
                    }
                }    
            }
            if (this.dialogSaveData.dialogProgress != null)
            {
                foreach (DialogProgress flag in saveData.dialogProgress)
                {
                    if (!string.IsNullOrEmpty(flag.dialogID))
                    {
                        this.dialogProgress[flag.dialogID] = flag.currentDialogIndex;
                    }
                }
            }
        }    
    }    
}
