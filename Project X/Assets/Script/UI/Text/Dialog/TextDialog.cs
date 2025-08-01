using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDialog : BaseText
{
    protected override void Start()
    {
        base.Start();
        EventManager.OP_EventManager.Subscribe<string>(NameEvent.Event_LoadDialogText, TypeWritterEffect);
    }
    public void TypeWritterEffect(string text)
    {
        StartCoroutine(RunEffect(text, this.m_TextMeshProUGUI));
    }   
    IEnumerator RunEffect(string textinput, TextMeshProUGUI textLabel)
    {
        float time = 0;
        int charindex = 0;
        while(textinput.Length > charindex)
        {
            time += Time.deltaTime * 50f;
            charindex = Mathf.FloorToInt(time);
            charindex = Mathf.Clamp(charindex, 0, textinput.Length);
            textLabel.text = textinput.Substring(0, charindex);
            yield return null;
        }
        textLabel.text = textinput;
    }    
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe<string>(NameEvent.Event_LoadDialogText, TypeWritterEffect);
    }
}
