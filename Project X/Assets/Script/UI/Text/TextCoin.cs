using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCoin : BaseText
{
    private MyGameManager myGameManager;
    protected override void Start()
    {
        this.myGameManager = GameObject.FindFirstObjectByType<MyGameManager>();
        base.Start();
        EventManager.OP_EventManager.Subscribe("LoadCoinText", Load);
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.myGameManager.CoinAmount.ToString() ;
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("LoadCoinText", Load);
    }
}
