using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCoin : BaseText
{
    private CoinManager myGameManager;
    protected override void Start()
    {
        this.myGameManager = GameObject.FindFirstObjectByType<CoinManager>();
        base.Start();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_LoadCoinText, Load);
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.myGameManager.CoinAmount.ToString() ;
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_LoadCoinText, Load);
    }
}
