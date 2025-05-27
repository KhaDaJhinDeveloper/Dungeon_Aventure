using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseText : MonoBehaviour,ILoadUI
{
    protected TextMeshProUGUI m_TextMeshProUGUI;
    protected virtual void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    public virtual void Load()
    {
       
    }
    public virtual void Loadinput(string textinput)
    {

    }
}
