using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReportSlotItem : BaseText
{
    protected override void Start()
    {
        base.Start();
        EventManager.OP_EventManager.Subscribe<string>("LoadSlotItemReportText", Loadinput);
    }
    public override void Loadinput(string textinput)
    {
        StartCoroutine(LoadTextreport(textinput));
    }
    IEnumerator LoadTextreport(string text)
    {
        m_TextMeshProUGUI.text = text;
        yield return new WaitForSecondsRealtime(0.75f);
        m_TextMeshProUGUI.text = " ";
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe<string>("LoadSlotItemReportText", Loadinput);
    }
}
