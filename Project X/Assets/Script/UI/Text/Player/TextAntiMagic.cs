using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAntiMagic : BaseText
{
    private PlayerStats playerStats;
    protected override void Start()
    {
        base.Start();
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
    }
    private void Update()
    {
        Load();
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.playerStats.AntiMagic.ToString() + "/" + this.playerStats.MaxAntiMagic.ToString();
    }
}
