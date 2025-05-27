using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHp : BaseText
{
    private PlayerStats playerStats;
    protected override void Start()
    {       
        base.Start();
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
        Load();
        EventManager.OP_EventManager.Subscribe("LoadHPText", Load);
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.playerStats.CurentHealth.ToString() + "/" + this.playerStats.MaxHealth.ToString();
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe("LoadHPText", Load);
    }
}
