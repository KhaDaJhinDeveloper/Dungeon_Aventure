using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDescribeItem : BaseText
{
    private ShopSystem shopManager;
    protected override void Start()
    {
        this.shopManager = GameObject.FindFirstObjectByType<ShopSystem>();
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        Load();
    }
    public override void Load()
    {

    }
}
