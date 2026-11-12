using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCraftingReport : BaseText
{
    protected override void Start()
    {
        base.Start();
        EventManager.Instance.Subscribe<string>(NameEvent.Event_LoadCraftingReportText, Loadinput);
    }
    public override void Loadinput(string textinput)
    {
        StartCoroutine(LoadTextreport(textinput));
    }
    IEnumerator LoadTextreport(string text)
    {
        m_TextMeshProUGUI.text = text;
        yield return new WaitForSecondsRealtime(0.75f);
        m_TextMeshProUGUI.text = null;
    }    
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<string>(NameEvent.Event_LoadCraftingReportText, Loadinput);
    }
}
