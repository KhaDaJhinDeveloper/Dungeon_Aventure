using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStateManager : Singleton<DialogStateManager>
{
    public static DialogStateManager dialogState_Instance;
    public HashSet<string> completedDialogs = new HashSet<string>();
    public Dictionary<string, bool> dialogFlags = new Dictionary<string, bool>();
    public DialogSaveData dialogSaveData = new DialogSaveData();
    protected override void Awake()
    {
        dialogState_Instance = this;
    }
    public void MarkDialogCompleted(string dialogID)
    {
        if(!string.IsNullOrEmpty(dialogID))
        {
            this.completedDialogs.Add(dialogID);
            SaveDialogState();
        }    
    }
    private void Update()
    {
        if (this.completedDialogs == null) Debug.Log("completedialog null");
        if (this.dialogFlags == null) Debug.Log("dialogflags null");
    }
    public bool CheckDialogCompleted(string dialogID)
    {
        return this.completedDialogs.Contains(dialogID);
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
        this.dialogSaveData.completedDialogs = new List<string>(this.completedDialogs);
        this.dialogSaveData.dialogFlags = new Dictionary<string, bool>(this.dialogFlags);
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
            this.completedDialogs = new HashSet<string>(saveData.completedDialogs);
            this.dialogFlags = new Dictionary<string, bool>(saveData.dialogFlags);
        }    
    }    
}
