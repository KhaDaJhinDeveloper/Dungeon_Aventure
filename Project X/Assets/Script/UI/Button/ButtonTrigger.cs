using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private Transform posPlayer;
    void Start()
    {
        this.posPlayer = GameObject.Find(NameManager.NAME_PLAYER).transform;
        this.gameObject.SetActive(false);
        EventManager.OP_EventManager.Subscribe("EventShowButton", Showbutton);
        EventManager.OP_EventManager.Subscribe("EventHiddenButton", HiddenButton);
    }
    void Showbutton()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = this.posPlayer.position;
    }
    void HiddenButton()
    {
        this.gameObject.SetActive(false);
    }    
}
