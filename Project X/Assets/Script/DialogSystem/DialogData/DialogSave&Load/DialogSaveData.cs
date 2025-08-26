using System.Collections.Generic;

[System.Serializable]
public class DialogFlags
{
    public string key;
    public bool value;
    public DialogFlags() { }
    public DialogFlags(string k, bool v) 
    {
        key = k;
        value = v;
    }
}
public class DialogProgress
{
    public string dialogID;
    public int currentDialogIndex;
    public DialogProgress() { }  
    public DialogProgress(string ID, int Index)
    {
        dialogID = ID;
        currentDialogIndex = Index;
    }
}

[System.Serializable]
public class DialogSaveData 
{
    public List<string> completedDialogs;
    public List<DialogFlags> dialogFlags;
    public List <DialogProgress> dialogProgress;
}
