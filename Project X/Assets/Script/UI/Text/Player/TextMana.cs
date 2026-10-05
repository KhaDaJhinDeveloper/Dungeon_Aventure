using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMana : BaseText
{
    private PlayerStats playerStats;
    protected override void Start()
    {
        base.Start();
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
        Load();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_LoadManaText, Load);
    }
    public override void Load()
    {
        m_TextMeshProUGUI.text = this.playerStats.Mana.ToString() + "/" + this.playerStats.MaxMana.ToString();
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_LoadManaText, Load);
    }
}
