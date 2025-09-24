using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseText : MonoBehaviour,ILoadUI
{
    protected TextMeshProUGUI m_TextMeshProUGUI;
    protected TextMeshPro m_Text;
    protected virtual void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        m_Text = GetComponent<TextMeshPro>();
    }
    protected virtual void Update()
    {
        
    }
    public virtual void Load()
    {
       
    }
    public virtual void Loadinput(string textinput)
    {

    }
}
