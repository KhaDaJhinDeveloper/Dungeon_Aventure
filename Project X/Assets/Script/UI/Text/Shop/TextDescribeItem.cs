using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDescribeItem : BaseText
{
    private ShopManager shopManager;
    protected override void Start()
    {
        this.shopManager = GameObject.FindFirstObjectByType<ShopManager>();
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        Load();
    }
    public override void Load()
    {
        if(this.shopManager.currentSlot != null)
            m_TextMeshProUGUI.text = this.shopManager.currentSlot.describe;
    }
}
