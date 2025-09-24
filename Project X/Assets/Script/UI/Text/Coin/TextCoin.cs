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
    }
    protected override void Update()
    {
        base.Update();
        Load();
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.myGameManager.CoinAmount.ToString() ;
    }
}
